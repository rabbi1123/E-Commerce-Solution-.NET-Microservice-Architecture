using AutoMapper;
using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;

namespace ShoppingCart.Application.Features.Cart.Handlers.Commands
{
	public class CheckoutCartHandler : IRequestHandler<CheckoutCart, Unit>
	{
		private readonly ICartHeaderRepository cartRepository;
		private readonly IMapper mapper;
		private readonly IPublishEndpoint publishEndpoint;

		public CheckoutCartHandler(ICartHeaderRepository cartRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
		{
			this.cartRepository = cartRepository;
			this.mapper = mapper;
			this.publishEndpoint = publishEndpoint;
		}
		public async Task<Unit> Handle(CheckoutCart request, CancellationToken cancellationToken)
		{
			var basket = await cartRepository.GetCartWithDetailsByUserId(request.BasketCheckout.UserName);
			if (basket == null)
			{
				return Unit.Value;
			}
			var eventMessage = mapper.Map<BasketCheckoutEvent>(request.BasketCheckout);
			eventMessage.TotalPrice = basket.TotalPrice;
			await publishEndpoint.Publish(eventMessage);
			await cartRepository.Delete(basket);
			return Unit.Value;
		}
	}
}
