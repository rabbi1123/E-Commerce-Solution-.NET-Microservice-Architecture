using ECommerce.Application.Features.Product.Handlers.Queries;
using ECommerce.Application.Features.Product.Requests.Queries;
using ECommerce.Application.Persistence.Contracts;
using Moq;
using ProductCatalog.Application.UnitTest.Support;
using DomainProduct = ECommerce.Domain.Entity.Product;

namespace ProductCatalog.Application.UnitTest.Features.Product.Handlers.Queries;

public class GetProductListRequestHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedProducts()
    {
        var products = new List<DomainProduct>
        {
            new DomainProduct
            {
                Id = "a",
                name = "One",
                price = 1,
                description = null,
                categoryName = null,
                imageUrl = null
            }
        };

        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetAll()).ReturnsAsync(products);

        var sut = new GetProductListRequestHandler(repo.Object, ProductHandlerMapperMocks.ForList());

        var result = (await sut.Handle(new GetProductListRequest(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("a", result[0].Id);
        Assert.Equal("One", result[0].name);
        Assert.Equal(1, result[0].price);
        repo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenEmpty_ReturnsEmptySequence()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetAll()).ReturnsAsync(new List<DomainProduct>());

        var sut = new GetProductListRequestHandler(repo.Object, ProductHandlerMapperMocks.ForList());

        var result = await sut.Handle(new GetProductListRequest(), CancellationToken.None);

        Assert.Empty(result);
    }
}
