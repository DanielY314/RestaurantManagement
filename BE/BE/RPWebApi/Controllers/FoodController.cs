using Microsoft.AspNetCore.Mvc;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class FoodController : Controller
    {
        [HttpGet]
        public IActionResult GetFood()
        {
            var c = new foodService();
            return Ok(c.GetFoods()); // List 가 Json 으로 Serialization 되서 Postman 혹은 client 로 리턴
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            var c = new foodService();
            return Ok(c.GetFoods(id)); // List 가 Json 으로 Serialization 되서 Postman 혹은 client 로 리턴
        }
    }
}
