using API.Data;
using API.Entities;
using AutoMapper;
using DatingApi.DTOs;
using DatingApi.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService  _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>>Register([FromBody] RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("Name was taken ");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user);
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhtotUrl = user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,

            }) ;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LogingDto logingDto)
        {
            var user = await _userManager.Users.Include(i=>i.Photos)
                .SingleOrDefaultAsync(x => x.UserName == logingDto.UserName);
            if (user == null) 
            {
                return Unauthorized();
            }

            var result = await _userManager.CheckPasswordAsync(user, logingDto.Password);   
            if (!result)
            {
                return Unauthorized("invalid password");
            }

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token =await _tokenService.CreateToken(user),
                PhtotUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Gender = user.Gender,
                KnownAs = user.KnownAs,
            });
        }

        private async Task<bool>UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(i=> i.UserName == userName.ToLower());
        }
    }
}
