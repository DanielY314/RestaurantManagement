namespace RPWebApi.Models
{
    public class FoodOrderItem
    {
        public int id { get; set; }
        public int food_id { get; set; }
        public int food_order_id { get; set; }
        public int count { get; set; }
    }
}