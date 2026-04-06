using Moq;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Handlers.Commands;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Commands;

public class DeleteCartCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeletesMappedCart()
    {
        var dto = new CartHeaderDto
        {
            Id = 3,
            UserId = "u",
            TotalPrice = 0,
            CartDetails = new List<CartDetailsDto>()
        };

        var repo = new Mock<ICartHeaderRepository>();

        var sut = new DeleteCartCommandHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderFromDto());

        await sut.Handle(new DeleteCartCommand(dto), CancellationToken.None);

        repo.Verify(r => r.Delete(It.Is<CartHeader>(h => h.Id == 3)), Times.Once);
    }
}
