using Moq;
using Order.Application.Features.Orders.Queries.GetOrdersList;
using Order.Application.Contracts.Persistence;
using Order.Application.UnitTest.Support;
using Order.Domain.Entity;

namespace Order.Application.UnitTest.Features.Orders.Queries.GetOrdersList;

public class GetOrderListQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedOrdersForUser()
    {
        var orders = new List<ProductOrder>
        {
            new()
            {
                Id = 1,
                UserName = "alice",
                TotalPrice = 10m,
                FirstName = "A",
                LastName = "L",
                EmailAddress = "a@b.com",
                AddressLine = "",
                Country = "",
                City = "",
                ZipCode = "",
                CardName = "",
                CardNumber = "",
                Expiration = "",
                CVV = "",
                PaymentMethod = 0
            }
        };

        var repo = new Mock<IOrderRepository>();
        repo.Setup(r => r.GetOrdersByUserName("alice")).ReturnsAsync(orders);

        var sut = new GetOrderListQueryHandler(repo.Object, OrderHandlerMapperMocks.ForOrderList());

        var result = await sut.Handle(new GetOrderListQuery("alice"), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("alice", result[0].UserName);
        Assert.Equal(10m, result[0].TotalPrice);
        repo.Verify(r => r.GetOrdersByUserName("alice"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoOrders_ReturnsEmptyList()
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(r => r.GetOrdersByUserName("nobody")).ReturnsAsync(Array.Empty<ProductOrder>());

        var sut = new GetOrderListQueryHandler(repo.Object, OrderHandlerMapperMocks.ForOrderList());

        var result = await sut.Handle(new GetOrderListQuery("nobody"), CancellationToken.None);

        Assert.Empty(result);
    }
}
