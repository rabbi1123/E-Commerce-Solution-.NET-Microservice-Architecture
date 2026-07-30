using MediatR;
using Order.Application.DTOs.Order;

namespace Order.Application.Features.Orders.Queries.GetOrdersList
{
	public record GetOrderListQuery(string userName) : IRequest<List<OrderVm>>;
}
