using ECommerce.Application.Features.Product.Handlers.Queries;
using ECommerce.Application.Features.Product.Requests.Queries;
using ECommerce.Application.Persistence.Contracts;
using Moq;
using ProductCatalog.Application.UnitTest.Support;
using DomainProduct = ECommerce.Domain.Entity.Product;

namespace ProductCatalog.Application.UnitTest.Features.Product.Handlers.Queries;

public class GetProductByIdRequestHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedProduct()
    {
        var product = new DomainProduct
        {
            Id = "xyz",
            name = "Item",
            price = 3,
            description = "desc",
            categoryName = "cat",
            imageUrl = "img"
        };

        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Get("xyz")).ReturnsAsync(product);

        var sut = new GetProductByIdRequestHandler(repo.Object, ProductHandlerMapperMocks.ForGetById());

        var dto = await sut.Handle(new GetProductByIdRequest("xyz"), CancellationToken.None);

        Assert.Equal("xyz", dto.Id);
        Assert.Equal("Item", dto.name);
        Assert.Equal(3, dto.price);
        repo.Verify(r => r.Get("xyz"), Times.Once);
    }
}
