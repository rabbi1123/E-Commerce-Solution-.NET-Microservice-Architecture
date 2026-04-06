using Moq;
using ShoppingCart.Application.Features.Cart.Handlers.Queries;
using ShoppingCart.Application.Features.Cart.Requests.Queries;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Queries;

public class GetCartListRequestHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedCarts()
    {
        var carts = new List<CartHeader>
        {
            new()
            {
                Id = 1,
                UserId = "u1",
                TotalPrice = 5,
                CartDetails = new List<CartDetails>()
            }
        };

        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.GetAllCartWithDetails()).ReturnsAsync(carts);

        var sut = new GetCartListRequestHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderList());

        var result = (await sut.Handle(new GetCartListRequest(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("u1", result[0].UserId);
        repo.Verify(r => r.GetAllCartWithDetails(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenEmpty_ReturnsEmptySequence()
    {
        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.GetAllCartWithDetails()).ReturnsAsync(new List<CartHeader>());

        var sut = new GetCartListRequestHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderList());

        var result = await sut.Handle(new GetCartListRequest(), CancellationToken.None);

        Assert.Empty(result);
    }
}
