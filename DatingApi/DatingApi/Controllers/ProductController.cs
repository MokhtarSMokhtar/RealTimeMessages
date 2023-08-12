using DatingApi.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductDAL.Entities;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddProduct([FromBody]Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving product: {ex.InnerException?.Message}");
            }
        }


        [HttpGet("get")]

        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var response = await _productRepository.GetProducts();
            return Ok(response);
        }
    }
}
