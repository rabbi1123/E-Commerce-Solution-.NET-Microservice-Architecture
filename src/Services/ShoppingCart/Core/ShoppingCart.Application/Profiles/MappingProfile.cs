using AutoMapper;
using Eventbus.Messages.Events;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartHeader, CartHeaderDto>()
                .ForMember(dest => dest.CartDetails, opt => opt.MapFrom(src => src.CartDetails)).ReverseMap();
            
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
