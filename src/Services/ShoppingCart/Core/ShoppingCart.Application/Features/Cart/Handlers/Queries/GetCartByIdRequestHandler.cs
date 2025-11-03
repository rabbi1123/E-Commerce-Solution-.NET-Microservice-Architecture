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
    public class GetCartByIdRequestHandler : IRequestHandler<GetCartByIdRequest, CartHeaderDto>
    {
        private readonly ICartHeaderRepository cartRepository;
        private readonly IMapper mapper;

        public GetCartByIdRequestHandler(ICartHeaderRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }
        public async Task<CartHeaderDto> Handle(GetCartByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await cartRepository.Get(request.Id);
            return mapper.Map<CartHeaderDto>(product);
        }
    }
}
