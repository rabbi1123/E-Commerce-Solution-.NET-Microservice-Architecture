using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Features.Product.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender sender;

        public ProductController(ISender sender) => this.sender = sender;

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var products = await sender.Send(new GetProductListRequest());
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var product = await sender.Send(new GetProductByIdRequest(id));
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
		[Authorize(Roles = "Administrator")]
		public async Task<ActionResult> Post([FromBody] ProductCreateDto value)
        {
            var result = await sender.Send(new AddProductCommand(value));
            return Ok(result);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ProductDto value)
        {
            await sender.Send(new UpdateProductCommand(value));
            return Ok();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await sender.Send(new DeleteProductCommand(id));
            return Ok();
        }
    }
}
