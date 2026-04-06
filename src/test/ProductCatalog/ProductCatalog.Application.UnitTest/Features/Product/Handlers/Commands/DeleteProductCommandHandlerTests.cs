using ECommerce.Application.Features.Product.Handlers.Commands;
using ECommerce.Application.Features.Product.Requests.Commands;
using ECommerce.Application.Persistence.Contracts;
using Moq;
using ProductCatalog.Application.UnitTest.Support;

namespace ProductCatalog.Application.UnitTest.Features.Product.Handlers.Commands;

public class DeleteProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeletesProductById()
    {
        var repo = new Mock<IProductRepository>();

        var sut = new DeleteProductCommandHandler(repo.Object, ProductHandlerMapperMocks.Unused());

        await sut.Handle(new DeleteProductCommand("to-remove"), CancellationToken.None);

        repo.Verify(r => r.Delete("to-remove"), Times.Once);
    }
}
