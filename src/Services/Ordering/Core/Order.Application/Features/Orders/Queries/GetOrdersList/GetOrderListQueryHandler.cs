using AutoMapper;
using MediatR;
using Order.Application.Contracts.Persistence;
using Order.Application.DTOs.Order;

namespace Order.Application.Features.Orders.Queries.GetOrdersList
{
	public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderVm>>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository;
			this.mapper = mapper;
		}
		public async Task<List<OrderVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
		{
			var orderList = await orderRepository.GetOrdersByUserName(request.userName);
			return mapper.Map<List<OrderVm>>(orderList);
		}
	}
}
