using Discount.API.Models;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository repository;

        public DiscountController(IDiscountRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDiscount>> Get(int productId)
        {
            var discount = await repository.GetDiscount(productId);
            return Ok(discount);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDiscount>> Create([FromBody] ProductDiscount discount)
        {
            await repository.CreateDiscount(discount);
            return CreatedAtRoute("GetDiscount", new { productId = discount.ProductId }, discount);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDiscount>> Update([FromBody] ProductDiscount discount)
        {
            return Ok(await repository.UpdateDiscount(discount));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(int productId)
        {
            return Ok(await repository.DeleteDiscount(productId));
        }
    }
}
