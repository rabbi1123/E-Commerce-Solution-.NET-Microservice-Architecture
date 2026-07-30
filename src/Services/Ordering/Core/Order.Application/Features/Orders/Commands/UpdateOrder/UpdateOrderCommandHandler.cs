using AutoMapper;
using MediatR;
using Order.Application.Contracts.Persistence;
using Order.Domain.Entity;

namespace Order.Application.Features.Orders.Commands.UpdateOrder
{
	public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository;
			this.mapper = mapper;
		}
		public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
		{
			var orderForUpdate = await orderRepository.GetByIdAsync(request.Id);
			if (orderForUpdate == null)
			{

			}
			mapper.Map(request, orderForUpdate, typeof(UpdateOrderCommand), typeof(ProductOrder));
			await orderRepository.UpdateAsync(orderForUpdate);
			return Unit.Value;
		}
	}
}
