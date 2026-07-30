using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs.Order;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.DeleteOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Application.Features.Orders.Queries.GetOrdersList;

namespace Order.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly ISender sender;

		public OrderController(ISender sender) => this.sender = sender;

		[HttpGet("{userName}")]
		public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrderByUserName(string userName)
		{
			var query = await sender.Send(new GetOrderListQuery(userName));
			return Ok(query);
		}

		[HttpPost(Name = "CheckoutOrder")]
		public async Task<ActionResult<int>> CheckoutOrder(CheckoutOrderCommand command)
		{
			var result = await sender.Send(command);
			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateOrder(string id, UpdateOrderCommand command)
		{
			var result = await sender.Send(command);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteOrder(int id)
		{
			await sender.Send(new DeleteOrderCommand(id));
			return Ok();
		}
	}
}
