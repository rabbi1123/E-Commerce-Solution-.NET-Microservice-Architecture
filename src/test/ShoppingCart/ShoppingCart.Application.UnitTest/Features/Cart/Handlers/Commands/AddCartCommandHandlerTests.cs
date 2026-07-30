using Moq;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Handlers.Commands;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Commands;

public class AddCartCommandHandlerTests
{
    [Fact]
    public async Task Handle_PersistsCartAndReturnsId()
    {
        var dto = new CartHeaderDto
        {
            UserId = "user-1",
            TotalPrice = 10,
            CartDetails = new List<CartDetailsDto>()
        };

        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.Add(It.IsAny<CartHeader>()))
            .ReturnsAsync((CartHeader c) =>
            {
                c.Id = 99;
                return c;
            });

        var sut = new AddCartCommandHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderFromDto());

        var id = await sut.Handle(new AddCartCommand(dto), CancellationToken.None);

        Assert.Equal(99, id);
        repo.Verify(r => r.Add(It.Is<CartHeader>(h => h.UserId == "user-1")), Times.Once);
    }
}
