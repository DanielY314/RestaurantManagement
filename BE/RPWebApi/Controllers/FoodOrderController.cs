using Microsoft.AspNetCore.Mvc;
using RPWebApi.Models;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodOrderController : Controller
    {
        private readonly FoodOrderService _foodOrderService;

        public FoodOrderController(FoodOrderService foodOrderService)
        {
            _foodOrderService = foodOrderService;
        }

        [HttpGet]
        public IActionResult GetFoodOrders()
        {
            return Ok(_foodOrderService.GetFoodOrders());
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodOrder(int id)
        {
            var result = _foodOrderService.GetFoodOrder(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddFoodOrder(FoodOrder fo)
        {
            try
            {
                bool result = _foodOrderService.AddFoodOrder(fo);
                if (result)
                    return StatusCode(StatusCodes.Status201Created);
                else
                    return BadRequest("Food Order could not be created");
            }
            catch (Exception ex) { return BadRequest(ex); }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFoodOrder(int id)
        {
            try
            {
                bool result = _foodOrderService.DeleteFoodOrder(id);
                if (result)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return BadRequest("Food Order could not be deleted");
            }
            catch (Exception ex) { return BadRequest(ex); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFoodOrder(int id, FoodOrder fo)
        {
            try
            {
                bool result = _foodOrderService.UpdateFoodOrder(id, fo);
                if (result)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return BadRequest("Food Order could not be updated");
            }
            catch (Exception ex) { return BadRequest(ex); }
        }
    }
}