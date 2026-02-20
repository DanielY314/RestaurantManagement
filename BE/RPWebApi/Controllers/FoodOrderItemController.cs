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
        private readonly foodService _foodService;
        private readonly FoodOrderItemService _foodOrderItemService;
        private readonly FoodOrderService _foodOrderService;

        public FoodOrderItemController(foodService foodService, FoodOrderItemService foodOrderItemService, FoodOrderService foodOrderService)
        {
            _foodService = foodService;
            _foodOrderItemService = foodOrderItemService;
            _foodOrderService = foodOrderService;
        }

        [HttpPost]
        public IActionResult AddFoodOrderItem(List<FoodOrderDTO> fo)
        {
            try
            {
                var foods = new List<FoodCustomerOrder>();
                foreach (var member in fo)
                {
                    Food food = _foodService.GetFood(member.name);
                    foods.Add(new FoodCustomerOrder
                    {
                        FoodType = food,
                        Count = member.amount,
                        SeatName = member.seatName
                    });
                }

                bool result = _foodOrderItemService.AddFoodOrderItem(foods);
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