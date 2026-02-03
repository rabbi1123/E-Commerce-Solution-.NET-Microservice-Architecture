using AutoMapper;
using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.API.EventBusConsumer
{
	public class CheckoutConsumer : IConsumer<BasketCheckoutEvent>
	{
		private readonly IMediator mediator;
		private readonly IMapper mapper;

		public CheckoutConsumer(IMediator mediator, IMapper mapper)
        {
			this.mediator = mediator;
			this.mapper = mapper;
		}
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
		{
			var command = mapper.Map<CheckoutOrderCommand>(context.Message);
			var result = await mediator.Send(command);
		}
	}
}
