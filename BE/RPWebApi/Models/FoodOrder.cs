namespace RPWebApi.Models
{
    public class FoodOrder
    {
        public int id { get; set; }
        public DateTime order_date { get; set; }
        public decimal total { get; set; }
        public string seatName { get; set; }
    }
}