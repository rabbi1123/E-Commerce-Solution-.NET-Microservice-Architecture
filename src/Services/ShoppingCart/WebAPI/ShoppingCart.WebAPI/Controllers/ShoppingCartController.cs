using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Features.Cart.Requests.Queries;

namespace ShoppingCart.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ISender sender;
        public ShoppingCartController(ISender sender) => this.sender = sender;

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var cart = await sender.Send(new GetCartListRequest());
            return Ok(cart);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var cart = await sender.Send(new GetCartByIdRequest(id));
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CartHeaderDto value)
        {
            var result = await sender.Send(new AddCartCommand(value));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] CartHeaderDto value)
        {
            var result = await sender.Send(new UpdateCartCommand(value));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cart = await sender.Send(new GetCartByIdRequest(id));
            await sender.Send(new DeleteCartCommand(cart));
            return Ok();
        }
    }
}
