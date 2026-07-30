using AutoMapper;
using Eventbus.Messages.Events;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.API.Mapping
{
	public class OrderProfile : Profile
	{
        public OrderProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
