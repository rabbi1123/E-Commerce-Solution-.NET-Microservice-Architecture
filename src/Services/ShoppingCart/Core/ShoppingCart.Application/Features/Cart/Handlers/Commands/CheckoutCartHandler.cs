using AutoMapper;
using EventBus.RabbitMQ;
using Eventbus.Messages.Events;
using MediatR;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;

namespace ShoppingCart.Application.Features.Cart.Handlers.Commands
{
	public class CheckoutCartHandler : IRequestHandler<CheckoutCart, Unit>
	{
		private readonly ICartHeaderRepository cartRepository;
		private readonly IMapper mapper;
		private readonly IEventBus eventBus;

		public CheckoutCartHandler(ICartHeaderRepository cartRepository, IMapper mapper, IEventBus eventBus)
		{
			this.cartRepository = cartRepository;
			this.mapper = mapper;
			this.eventBus = eventBus;
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
			await eventBus.PublishAsync(eventMessage, cancellationToken);
			await cartRepository.Delete(basket);
			return Unit.Value;
		}
	}
}
