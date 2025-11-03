using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Features.Cart.Handlers.Commands
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, Unit>
    {
        private readonly ICartHeaderRepository cartRepository;
        private readonly IMapper mapper;

        public UpdateCartCommandHandler(ICartHeaderRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = mapper.Map<CartHeader>(request.Cart);
            await cartRepository.Update(cart);
            return Unit.Value;
        }
    }
}
