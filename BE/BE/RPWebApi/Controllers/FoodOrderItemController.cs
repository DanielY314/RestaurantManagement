using Microsoft.AspNetCore.Mvc;
using RPWebApi.Business;
using RPWebApi.Dto;
using RPWebApi.Models;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class FoodOrderItemController : Controller
    {
        [HttpPost]
        public IActionResult AddFoodOrderItem(List<FoodOrderDTO> fo)
        {
            try
            {
                // DTO object 를 FoodOrderItem object 로 바꿔야 한다.
                // 혹은 DTO object 를 이용해서 FoodOrderItem object 를 생성해야 한다.
                var p = new foodService();
                var foods = new List<FoodCustomerOrder>();
                foreach(var member in fo)
                {
                    Food food = p.GetFood(member.name);
                    foods.Add(new FoodCustomerOrder
                    { 
                          FoodType = food,
                          Count = member.amount,
                          SeatName = member.seatName
                    });
                }

                var f = new FoodOrderItemService();
                bool result = f.AddFoodOrderItem(foods);

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
