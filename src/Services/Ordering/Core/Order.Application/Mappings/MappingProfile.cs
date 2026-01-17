using AutoMapper;
using Order.Application.DTOs.Order;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
