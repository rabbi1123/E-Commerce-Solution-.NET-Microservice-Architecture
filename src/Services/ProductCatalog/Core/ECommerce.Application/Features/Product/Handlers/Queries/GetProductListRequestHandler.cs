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
    public class GetProductListRequestHandler : IRequestHandler<GetProductListRequest, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductListRequestHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductListRequest request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetAll();
            return mapper.Map<List<ProductDto>>(productList);
        }
    }
}
