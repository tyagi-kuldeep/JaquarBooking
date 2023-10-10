using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiceController : ControllerBase
    {
        [HttpGet]
        [Route("Hello")]
        //api/Service/Hello
        //[HttpPost]
        //[HttpPut]
        //[HttpPatch]
        //[HttpDelete]
        public IActionResult Test()
        {
            return Content("working fine");
        }

        [HttpGet]
        [Route("GetProductData")]
        public IActionResult GetProductData()
        {
            List<ProductTest> obj=new List<ProductTest>();
            obj.Add(new ProductTest
            {
                Id=1,
                Name="IPhone",
                Sku="SI001",
                Price=100000.00m
            });
            obj.Add(new ProductTest
            {
                Id = 2,
                Name = "Samsung",
                Sku = "SI001",
                Price = 50000m
            });
            obj.Add(new ProductTest
            {
                Id = 3,
                Name = "Redmi",
                Sku = "SI003",
                Price = 425m
            });
            obj.Add(new ProductTest
            {
                Id = 4,
                Name = "XI",
                Sku = "SI004",
                Price = 100000.00m
            });
            return Ok(obj);
        }

        public class ProductTest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Sku { get; set; }
            public decimal Price { get; set; }
        }


    }
}
