using Microsoft.AspNetCore.Mvc;
using MultiTenant.Services;
using MultiTenant.Services.Dtos;

namespace MultiTenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var result = productService.GetProducts();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CreatepProductRequest req)
        {
            var result = productService.CreateProduct(req);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = productService.DeleteProduct(id);
            return Ok(result);
        }
    }
}
