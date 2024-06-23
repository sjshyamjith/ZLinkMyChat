using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyChatApi.Dto;
using MyChatApi.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            UserDto userDto = new UserDto();
            userDto.Email = user.Email;
            userDto.Name = user.Name;
            userDto.Id = user.Id;
            return Ok(new ApiResponseDTO<UserDto>
            {
                StatusCode = 200,
                Data = userDto,
                Message = "User Details"
            });
        }
        
        [HttpGet]
        [Route("all/{userId}")]
        public async Task<IActionResult> GetAllUsers(string userId)
        {
            var users = await _userRepository.GetAllUsers(userId);
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UserRegistration(UserRegisterDto data)
        {
            try
            {
                var user = await _userRepository.AddAsync(data);
                UserDto userDto = new UserDto();
                userDto.Email = user.Email;
                userDto.Name = user.Name;
                userDto.Id = user.Id;
                //create token and add
                return Ok(new
                {
                    token = "text_token",
                    email = user.Email,
                    name = user.Name,
                    id = user.Id
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authentication(UserLoginDto data)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(data.Email);
                if (user != null && user.Password == data.Password)
                {
                    //create token and add
                    return Ok(new
                    {
                        token = "text_token",
                        email = user.Email,
                        name = user.Name,
                        id = user.Id
                    });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
