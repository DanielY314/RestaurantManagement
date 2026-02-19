using Microsoft.AspNetCore.Mvc;
using RPWebApi.Dto;
using RPWebApi.Models;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly userService _userService;

        public UserController(userService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register(UserResgisterDTO dto)
        {
            try
            {
                if (_userService.UserExist(dto.UserName))
                    return BadRequest("User already exist");

                var user = new User { Username = dto.UserName };
                var isCreated = _userService.AddUser(user, dto.Password);
                if (isCreated)
                    return Ok($"{dto.UserName} user created successfully");
                else
                    return BadRequest("Failed to create");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                if (!_userService.UserExist(dto.UserName))
                    return BadRequest("User not found");

                var user = _userService.GetUser(dto.UserName);
                if (!_userService.VerifyPasswordHash(dto.Password, user.passwordHash, user.passwordSalt))
                    return BadRequest("Invalid password");

                var token = _userService.GenerateJwtToken(user.Username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}