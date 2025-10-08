using AutoMapper;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Persistence.Contracts;
using ECommerce.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Product.Handlers.Commands
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, string>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<string> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<ECommerce.Domain.Entity.Product>(request.Product);
            //product.Id = 
            product = await productRepository.Add(product);
            return product.Id;
        }
    }
}
