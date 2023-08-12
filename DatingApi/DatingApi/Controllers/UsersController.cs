using API.Data;
using API.Entities;
using AutoMapper;
using DatingApi.Controllers;
using DatingApi.DTOs;
using DatingApi.Extentions;
using DatingApi.Helper;
using DatingApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper,
            IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
            _userRepository = userRepository;
        }

      
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>>GetUsers([FromQuery]UserPramas userPramas)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            userPramas.CurrentUsername = currentUser.UserName;
            if(string.IsNullOrEmpty(userPramas.Gender)) 
            {
                userPramas.Gender = currentUser.Gender == "male" ? "female" : "male";
            }
            var users = await _userRepository.GetUsersAsync(userPramas);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,
                users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
     
        [HttpGet("{username}")]         
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var member = await _userRepository.GetMemberAsync(username);
            return Ok(member);
        }
        [HttpPut]
        public async Task<ActionResult>UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            var userName = User.GetUserName();
            var user = await _userRepository.GetUserByUsernameAsync(userName);
      
            if (user == null)
            {
                return NotFound();
            }   
            _mapper.Map(memberUpdateDto, user);
            if(await _userRepository.SaveAllAsync()) {
                return NoContent();
            }
            return BadRequest();
        }


        [HttpPost("add-Photo")]
        public async Task<ActionResult<PhotoDto>>AddNewPhoto(IFormFile file)
        {
            var userName = User.GetUserName();
            var user = await _userRepository.GetUserByUsernameAsync(userName);
            if (userName == null) 
            {
                return NotFound();
            }
            var result = await _photoService.AddPhotoAsync(file);
            if(result.Error != null) 
            {
                return BadRequest(result.Error);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                publicId = result.PublicId,
                IsMain = user.Photos.Count > 0 ? false : true,


            };
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return _mapper.Map<PhotoDto>(photo);
            }
            return BadRequest();
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult>SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync( User.GetUserName());
            if (user == null) 
            {
                return NotFound();
            }
            var photo = user.Photos.FirstOrDefault(p=>p.Id == photoId);
            if(photo == null)
            {
                return NotFound();
            }
            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.publicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.publicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }

    }
}
