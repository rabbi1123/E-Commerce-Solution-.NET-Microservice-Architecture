using MediatR;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
	public record DeleteOrderCommand(int Id) : IRequest;
}
