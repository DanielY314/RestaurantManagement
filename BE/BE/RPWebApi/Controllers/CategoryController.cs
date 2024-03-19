using Microsoft.AspNetCore.Mvc;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult GetCategory()
        {
            var c = new categoryService();
            return Ok(c.GetCategories()); // List 가 Json 으로 Serialization 되서 Postman 혹은 client 로 리턴
        }
    }
}
