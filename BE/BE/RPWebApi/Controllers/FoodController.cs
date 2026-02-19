using Microsoft.AspNetCore.Mvc;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodController : Controller
    {
        private readonly foodService _foodService;

        public FoodController(foodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public IActionResult GetFood()
        {
            return Ok(_foodService.GetFoods());
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            return Ok(_foodService.GetFoods(id));
        }
    }
}