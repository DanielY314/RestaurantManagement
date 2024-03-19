using Microsoft.AspNetCore.Mvc;
using RPWebApi.Models;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class FoodOrderController : Controller
    {
        [HttpPost]
        public IActionResult AddFoodOrder(FoodOrder fo)
        {
            try
            {
                var f = new FoodOrderService();
                bool result = f.AddFoodOrder(fo);

                if(result)
                    return StatusCode(StatusCodes.Status201Created);
                else
                    return BadRequest("Food Order could not be created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFoodOrder(int id)
        {
            try
            {
                var f = new FoodOrderService();
                bool result = f.DeleteFoodOrder(id);

                if (result)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return BadRequest("Food Order could not be deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFoodOrder(int id, FoodOrder fo)
        {
            try
            {
                var f = new FoodOrderService();
                bool result = f.UpdateFoodOrder(id, fo);

                if (result)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return BadRequest("Food Order could not be updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
