using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using Moq;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Cart.Handlers.Commands;
using ShoppingCart.Application.Features.Cart.Requests.Commands;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Application.UnitTest.Support;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Features.Cart.Handlers.Commands;

public class CheckoutCartHandlerTests
{
    [Fact]
    public async Task Handle_WhenNoBasket_ReturnsUnitWithoutPublishOrDelete()
    {
        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.GetCartWithDetailsByUserId("ghost"))
            .Returns(Task.FromResult<CartHeader>(null!));

        var publish = new Mock<IPublishEndpoint>();

        var sut = new CheckoutCartHandler(repo.Object, CartHandlerMapperMocks.ForCheckout(), publish.Object);

        var basketCheckout = new BasketCheckout { UserName = "ghost" };
        var result = await sut.Handle(new CheckoutCart(basketCheckout), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        publish.Verify(
            p => p.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
        repo.Verify(r => r.Delete(It.IsAny<CartHeader>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenBasketExists_PublishesEventWithBasketTotalPriceAndDeletesBasket()
    {
        var basket = new CartHeader
        {
            Id = 7,
            UserId = "alice",
            TotalPrice = 123.45,
            CartDetails = new List<CartDetails>()
        };

        var repo = new Mock<ICartHeaderRepository>();
        repo.Setup(r => r.GetCartWithDetailsByUserId("alice")).ReturnsAsync(basket);

        BasketCheckoutEvent? published = null;
        var publish = new Mock<IPublishEndpoint>();
        publish
            .Setup(p => p.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((msg, _) => published = (BasketCheckoutEvent)msg)
            .Returns(Task.CompletedTask);

        var checkoutDto = new BasketCheckout
        {
            UserName = "alice",
            TotalPrice = 0,
            FirstName = "A",
            LastName = "B",
            EmailAddress = "a@b.com",
            AddressLine = "1",
            Country = "US",
            City = "NY",
            ZipCode = "10001",
            CardName = "n",
            CardNumber = "1",
            Expiration = "12/30",
            CVV = "1",
            PaymentMethod = 1
        };

        var sut = new CheckoutCartHandler(repo.Object, CartHandlerMapperMocks.ForCheckout(), publish.Object);

        var result = await sut.Handle(new CheckoutCart(checkoutDto), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        Assert.NotNull(published);
        Assert.Equal(123.45, published!.TotalPrice);
        Assert.Equal("alice", published.UserName);
        publish.Verify(
            p => p.Publish(It.IsAny<BasketCheckoutEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
        repo.Verify(r => r.Delete(basket), Times.Once);
    }
}
