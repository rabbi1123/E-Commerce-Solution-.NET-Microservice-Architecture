using MediatR;
using Moq;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Handlers.Commands;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Commands;

public class UpdateCartCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsRepositoryUpdate()
    {
        var dto = new CartHeaderDto
        {
            Id = 5,
            UserId = "u",
            TotalPrice = 20,
            CartDetails = new List<CartDetailsDto>()
        };

        var repo = new Mock<ICartHeaderRepository>();

        var sut = new UpdateCartCommandHandler(repo.Object, CartHandlerMapperMocks.ForCartHeaderFromDto());

        var result = await sut.Handle(new UpdateCartCommand(dto), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        repo.Verify(r => r.Update(It.Is<CartHeader>(h => h.Id == 5 && h.TotalPrice == 20)), Times.Once);
    }
}
