using ECommerce.Application.Features.Product.Handlers.Commands;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Persistence.Contracts;
using ECommerce.Application.DTOs.Product;
using Moq;
using ProductCatalog.Application.UnitTest.Support;
using DomainProduct = ECommerce.Domain.Entity.Product;

namespace ProductCatalog.Application.UnitTest.Features.Product.Handlers.Commands;

public class AddProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_PersistsProductAndReturnsId()
    {
        var createDto = new ProductCreateDto
        {
            name = "Widget",
            price = 9.99,
            description = "A widget",
            categoryName = "Tools",
            imageUrl = "https://x/img.png"
        };

        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Add(It.IsAny<DomainProduct>()))
            .ReturnsAsync((DomainProduct p) =>
            {
                p.Id = "generated-id";
                return p;
            });

        var sut = new AddProductCommandHandler(repo.Object, ProductHandlerMapperMocks.ForAdd());

        var id = await sut.Handle(new AddProductCommand(createDto), CancellationToken.None);

        Assert.Equal("generated-id", id);
        repo.Verify(r => r.Add(It.Is<DomainProduct>(p =>
            p.name == "Widget" && p.price == 9.99 && p.categoryName == "Tools")), Times.Once);
    }
}
