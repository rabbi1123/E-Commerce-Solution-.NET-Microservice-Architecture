using AutoMapper;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Features.Product.Requests.Queries;
using ECommerce.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Product.Handlers.Queries
{
    public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, ProductDto>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductByIdRequestHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await productRepository.Get(request.Id);
            return mapper.Map<ProductDto>(product);
        }
    }
}
