using AutoMapper;
using Order.Application.DTOs.Order;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Domain.Entity;

namespace Order.Application.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<ProductOrder, OrderVm>().ReverseMap();
			CreateMap<ProductOrder, CheckoutOrderCommand>().ReverseMap();
			CreateMap<ProductOrder, UpdateOrderCommand>().ReverseMap();
		}
	}
}
