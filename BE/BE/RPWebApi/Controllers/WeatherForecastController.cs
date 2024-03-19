using Microsoft.AspNetCore.Mvc;
using RPWebApi.Models;
using RPWebApi.Services;

namespace RPWebApi.Controllers
{
    [ApiController] // data serialization, deserialization 을 자동으로 프로세스
    [Route("[controller]")] // client 가 어떤 신호를 보냈을 때 server 가 이 해당 클래스로 와서 실행
    // [controller] 라는 이름은 아래의 클래스 이름에서 Controller 라는 단어를 뺀 이름을 의미
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        static List<Product> _products = new List<Product>()
            {
                new Product() { ProductId = 1, ProductName = "Laptop", ProductPrice = 99.23M },
                new Product() { ProductId = 2, ProductName = "Desktop", ProductPrice = 55.34M }
            };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet] // web api 에서 어떤 방식으로 호출되는 것인가를 정의: Action
        // Action 에는 네 가지 필수 action
        // 1. HttpGet: client 가 자료를 조회하는 Action
        // 2. HttpPost: client 가 자료를 입력하는 Action
        // 3. HttpPut: client 가 자료를 수정하는 Action
        // 4. HttpDelete: client 가 자료를 삭제하는 Action
        public IEnumerable<WeatherForecast> sdfdsdsgffdsgfdgfdgfdgfd()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray(); // "자동으로" C# 의 DataType 이 Json 으로 변경되어 리턴 (Data Serialization: C# 의 datatype 이 다른 형식의 data type 으로 변환)
        }

        [HttpGet("product")] // 별도의 routing
        public IActionResult GetProduct()
        {
            var test = new categoryService();
            test.GetCategories();
            return Ok(_products);
            // return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult Post(Product product) // json 이라는 type 이 자동으로 c# type으로 변환: deserialization
        {
            try
            {
                _products.Add(product);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpDelete("{productId}")] // {productId} 라는 파라미커가 아래의 파라미터 값에 매칭
        public IActionResult Delete(int productId)
        {
            try
            {
                var product = _products.Where(x => x.ProductId == productId).FirstOrDefault<Product>();
                if (product != null)
                {
                    _products.Remove(product);
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return Ok("No product");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{productId}")] // {productId} 라는 파라미커가 아래의 파라미터 값에 매칭
        public IActionResult Update(int productId, Product value)
        {
            try
            {
                var product = _products.Where(x => x.ProductId == productId).FirstOrDefault<Product>();
                if (product != null)
                {
                    // update
                    product.ProductName = value.ProductName.ToUpper();
                    product.ProductPrice = value.ProductPrice;

                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return Ok("No product");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}