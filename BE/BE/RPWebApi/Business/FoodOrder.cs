using RPWebApi.Models;

namespace RPWebApi.Business
{
    public class FoodCustomerOrder
    {
        public Food FoodType { get; set; }
        public int Count { get; set; }
        public string SeatName { get; set; }
    }
}
