using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Extentions;
using DatingApi.Helper;
using DatingApi.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikesController(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;

        }
        [HttpPost("{username}")]
        public async Task<ActionResult>Addlike(string userName)
        {
            var sourceUserId = int.Parse( User.GetUserId());
            var likedUser = await _userRepository.GetUserByUsernameAsync(userName);
            var sourceUser = await _likesRepository.GetUserWithLike(sourceUserId);
            if(likedUser == null)
            {
                return NotFound();
            }
            if( sourceUser.UserName == userName) 
            {
                return BadRequest("you cannot like yourself");
            }
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if(userLike != null)
            {
                return BadRequest("you already like this user");
            };
            userLike = new UserLike
            {
                TargetUserId = likedUser.Id,
                SourceUserId = sourceUser.Id,
            };
            sourceUser.LikedUsers.Add(userLike);
            if(await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to like this user ");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikeParams likeParams)
        {
            likeParams.UserId = int.Parse( User.GetUserId());
            var users = await _likesRepository.GetUserLike(likeParams);

            Response.AddPaginationHeader(new PaginationHeader
                (
                    users.CurrentPage, users.PageSize,
                    users.TotalCount,
                    users.TotalPages
                ));
                
            return Ok(users);
        }
    }
}
