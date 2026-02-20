using Microsoft.AspNetCore.Mvc;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly categoryService _categoryService;

        public CategoryController(categoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            return Ok(_categoryService.GetCategories());
        }
    }
}