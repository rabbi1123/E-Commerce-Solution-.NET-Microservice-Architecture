using AutoMapper;
using MediatR;
using Order.Application.Contracts.Persistence;
using Order.Domain.Entity;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder
{
	public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository;
			this.mapper = mapper;
		}
		public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
		{
			var entity = mapper.Map<ProductOrder>(request);
			var order = await orderRepository.AddAsync(entity);
			return order.Id;
		}
	}
}
