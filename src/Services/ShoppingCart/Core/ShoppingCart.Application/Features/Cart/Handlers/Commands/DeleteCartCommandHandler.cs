using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.Features.Cart.Handlers.Commands
{
	public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand>
	{
		private readonly ICartHeaderRepository cartRepository;
		private readonly IMapper mapper;

		public DeleteCartCommandHandler(ICartHeaderRepository cartRepository, IMapper mapper)
		{
			this.cartRepository = cartRepository;
			this.mapper = mapper;
		}
		public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
		{
			var cart = mapper.Map<CartHeader>(request.Cart);
			await cartRepository.Delete(cart);
		}
	}
}
