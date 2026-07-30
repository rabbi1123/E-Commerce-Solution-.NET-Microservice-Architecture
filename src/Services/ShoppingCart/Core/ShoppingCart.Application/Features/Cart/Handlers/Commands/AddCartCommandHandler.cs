using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.Features.Cart.Handlers.Commands
{
	public class AddCartCommandHandler : IRequestHandler<AddCartCommand, int>
	{
		private readonly ICartHeaderRepository cartRepository;
		private readonly IMapper mapper;

		public AddCartCommandHandler(ICartHeaderRepository cartRepository, IMapper mapper)
		{
			this.cartRepository = cartRepository;
			this.mapper = mapper;
		}

		public async Task<int> Handle(AddCartCommand request, CancellationToken cancellationToken)
		{
			var cart = mapper.Map<CartHeader>(request.Cart);
			cart = await cartRepository.Add(cart);
			return cart.Id;
		}
	}
}
