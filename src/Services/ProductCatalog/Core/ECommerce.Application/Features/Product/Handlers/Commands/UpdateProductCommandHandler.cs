using AutoMapper;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Product.Handlers.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            //var product = await productRepository.Get(request.Product.Id);
            //mapper.Map(request.Product, product);

            var product = mapper.Map<ECommerce.Domain.Entity.Product>(request.Product);
            await productRepository.Update(product.Id,product);
            return Unit.Value;
        }
    }
}
