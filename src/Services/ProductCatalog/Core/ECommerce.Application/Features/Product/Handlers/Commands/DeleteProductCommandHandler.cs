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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public DeleteProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            //ECommerce.Domain.Entity.Product product = await productRepository.Get(request.Id);
            //product.Status = "inactive";
            await productRepository.Delete(request.Id);
        }
    }
}
