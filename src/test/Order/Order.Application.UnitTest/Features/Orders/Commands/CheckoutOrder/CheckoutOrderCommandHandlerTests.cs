using Moq;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Contracts.Persistence;
using Order.Application.UnitTest.Support;
using Order.Domain.Entity;

namespace Order.Application.UnitTest.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_MapsCommandToEntity_PersistsAndReturnsNewId()
    {
        var command = new CheckoutOrderCommand(
            UserName: "alice",
            TotalPrice: 99.50m,
            FirstName: "A",
            LastName: "B",
            EmailAddress: "a@b.com",
            AddressLine: "1 St",
            Country: "US",
            City: "NYC",
            ZipCode: "10001",
            CardName: "Alice",
            CardNumber: "4111",
            Expiration: "12/30",
            CVV: "123",
            PaymentMethod: 1);

        ProductOrder? added = null;
        var repo = new Mock<IOrderRepository>();
        repo
            .Setup(r => r.AddAsync(It.IsAny<ProductOrder>()))
            .Callback<ProductOrder>(e => added = e)
            .ReturnsAsync((ProductOrder e) =>
            {
                e.Id = 7;
                return e;
            });

        var sut = new CheckoutOrderCommandHandler(repo.Object, OrderHandlerMapperMocks.ForCheckout());

        var id = await sut.Handle(command, CancellationToken.None);

        Assert.Equal(7, id);
        Assert.NotNull(added);
        Assert.Equal("alice", added!.UserName);
        Assert.Equal(99.50m, added.TotalPrice);
        Assert.Equal("a@b.com", added.EmailAddress);
        repo.Verify(r => r.AddAsync(It.IsAny<ProductOrder>()), Times.Once);
    }
}
