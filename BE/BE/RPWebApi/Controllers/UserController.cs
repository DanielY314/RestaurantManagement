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
        [HttpPost]
        public IActionResult AddUser(UserResgisterDTO dto) //deserialization
        {
            try { 
                var userService = new userService();

                if (userService.UserExist(dto.UserName))
                {
                    return BadRequest("User alredy exist");
                }

                //DTO를 User Object 로 전환
                var user = new User
                {
                    Username = dto.UserName
                };


                var isCreated = userService.AddUser(user, dto.Password);
                if(isCreated)
                {
                    return Ok($"{dto.UserName} user created successfully");
                }
                else
                {
                    return BadRequest("Failed to create");
                }
                    //return Ok(c.GetCategories()); // List 가 Json 으로 Serialization 되서 Postman 혹은 client 로 리턴
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
