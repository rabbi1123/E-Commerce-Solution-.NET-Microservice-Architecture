using AutoMapper;
using MediatR;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Requests.Queries;
using ShoppingCart.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Features.Cart.Handlers.Queries
{
    public class GetCartListRequestHandler : IRequestHandler<GetCartListRequest, IEnumerable<CartHeaderDto>>
    {
        private readonly ICartHeaderRepository cartRepository;
        private readonly IMapper mapper;

        public GetCartListRequestHandler(ICartHeaderRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<CartHeaderDto>> Handle(GetCartListRequest request, CancellationToken cancellationToken)
        {
            var cartList = await cartRepository.GetAll();
            return mapper.Map<List<CartHeaderDto>>(cartList);
        }
    }
}
