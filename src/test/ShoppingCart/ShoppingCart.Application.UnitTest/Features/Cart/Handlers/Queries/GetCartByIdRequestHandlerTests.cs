using Moq;
using ShoppingCart.Application.Features.Cart.Handlers.Queries;
using ShoppingCart.Application.Features.Cart.Requests.Queries;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Queries;

public class GetCartByIdRequestHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedCart()
    {
        var cart = new CartHeader
        {
            Id = 42,
            UserId = "u",
            TotalPrice = 8,
            CartDetails = new List<CartDetails>()
        };

        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.GetCartWithDetails(42)).ReturnsAsync(cart);

        var sut = new GetCartByIdRequestHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderGetById());

        var dto = await sut.Handle(new GetCartByIdRequest(42), CancellationToken.None);

        Assert.Equal(42, dto.Id);
        Assert.Equal("u", dto.UserId);
        Assert.Equal(8, dto.TotalPrice);
        repo.Verify(r => r.GetCartWithDetails(42), Times.Once);
    }
}
