using Moq;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Application.Contracts.Persistence;
using Order.Application.UnitTest.Support;
using Order.Domain.Entity;

namespace Order.Application.UnitTest.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenOrderExists_UpdatesRepository()
    {
        var existing = new ProductOrder
        {
            Id = 3,
            UserName = "old",
            TotalPrice = 1m,
            FirstName = "O",
            LastName = "L",
            EmailAddress = "old@x.com",
            AddressLine = "old",
            Country = "old",
            City = "old",
            ZipCode = "old",
            CardName = "old",
            CardNumber = "old",
            Expiration = "old",
            CVV = "old",
            PaymentMethod = 0
        };

        var command = new UpdateOrderCommand(
            Id: 3,
            UserName: "bob",
            TotalPrice: 200m,
            FirstName: "B",
            LastName: "Ob",
            EmailAddress: "bob@x.com",
            AddressLine: "2 Ave",
            Country: "CA",
            City: "TO",
            ZipCode: "M5V",
            CardName: "Bob",
            CardNumber: "4222",
            Expiration: "01/31",
            CVV: "999",
            PaymentMethod: 2);

        var repo = new Mock<IOrderRepository>();
        repo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);

        var sut = new UpdateOrderCommandHandler(repo.Object, OrderHandlerMapperMocks.ForUpdate());

        await sut.Handle(command, CancellationToken.None);

        Assert.Equal("bob", existing.UserName);
        Assert.Equal(200m, existing.TotalPrice);
        Assert.Equal("bob@x.com", existing.EmailAddress);
        repo.Verify(r => r.UpdateAsync(existing), Times.Once);
    }
}
