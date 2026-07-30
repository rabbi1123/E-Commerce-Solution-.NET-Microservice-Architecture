using AutoMapper;
using MediatR;
using Order.Application.Contracts.Persistence;
using Order.Application.Exceptions;
using Order.Domain.Entity;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository;
			this.mapper = mapper;
		}
		public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var orderToDelete = await orderRepository.GetByIdAsync(request.Id);
			if (orderToDelete == null)
			{
				throw new NotFoundException(nameof(ProductOrder), request.Id);
			}
			await orderRepository.DeleteAsync(orderToDelete);
			return Unit.Value;
		}
	}
}
