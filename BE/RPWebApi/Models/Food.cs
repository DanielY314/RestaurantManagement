namespace RPWebApi.Models
{
    public class Food
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public decimal original_price { get; set; }

        public decimal sales_price { get; set; }

    }
}
