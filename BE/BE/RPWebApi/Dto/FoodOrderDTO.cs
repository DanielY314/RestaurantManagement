using System.ComponentModel.DataAnnotations;

namespace RPWebApi.Dto
{
    //public class FoodOrderDTO
    //{
    //    [Required(ErrorMessage ="Food Name cannot be empty")]
    //    public string Food { get; set; }

    //    [Required(ErrorMessage = "Food Count cannot be empty")]
    //    public int Count { get; set; }
    //}

    public class FoodOrderDTO
    {
        [Required(ErrorMessage = "Food Name cannot be empty")]
        public string name { get; set; }

        [Required(ErrorMessage = "Food amount cannot be empty")]
        public int amount { get; set; }

        [Required(ErrorMessage = "seat name cannot be empty")]
        public string seatName { get; set; }
    }
}
