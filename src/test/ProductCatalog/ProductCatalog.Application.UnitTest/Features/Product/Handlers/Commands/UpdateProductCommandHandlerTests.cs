using ECommerce.Application.Features.Product.Handlers.Commands;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Persistence.Contracts;
using ECommerce.Application.DTOs.Product;
using MediatR;
using Moq;
using ProductCatalog.Application.UnitTest.Support;
using DomainProduct = ECommerce.Domain.Entity.Product;

namespace ProductCatalog.Application.UnitTest.Features.Product.Handlers.Commands;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsRepositoryUpdateWithMappedProduct()
    {
        var dto = new ProductDto
        {
            Id = "p-1",
            name = "Updated",
            price = 12.5,
            description = "d",
            categoryName = "c",
            imageUrl = "u"
        };

        var repo = new Mock<IProductRepository>();

        var sut = new UpdateProductCommandHandler(repo.Object, ProductHandlerMapperMocks.ForUpdate());

        var result = await sut.Handle(new UpdateProductCommand(dto), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        repo.Verify(r => r.Update("p-1", It.Is<DomainProduct>(p => p.Id == "p-1" && p.name == "Updated" && p.price == 12.5)), Times.Once);
    }
}
