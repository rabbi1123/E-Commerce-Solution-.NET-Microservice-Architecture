using AutoMapper;
using Eventbus.Messages.Events;
using Moq;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.UnitTest.Support;

internal static class CartHandlerMapperMocks
{
    public static IMapper ForCartHeaderFromDto()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<CartHeader>(It.IsAny<CartHeaderDto>()))
            .Returns((CartHeaderDto d) => new CartHeader
            {
                Id = d.Id,
                UserId = d.UserId,
                TotalPrice = d.TotalPrice,
                CartDetails = d.CartDetails?.Select(cd => new CartDetails
                {
                    Id = cd.Id,
                    ProductId = cd.ProductId,
                    ProductName = cd.ProductName,
                    Count = cd.Count,
                    UnitPrice = cd.UnitPrice,
                    CardHeaderId = cd.CardHeaderId
                }).ToList()
            });
        return m.Object;
    }

    public static IMapper ForCartHeaderList()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<List<CartHeaderDto>>(It.IsAny<object>()))
            .Returns((object src) =>
            {
                if (src is not IEnumerable<CartHeader> list)
                    return new List<CartHeaderDto>();
                return list.Select(c => new CartHeaderDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    TotalPrice = c.TotalPrice,
                    CartDetails = c.CartDetails?.Select(cd => new CartDetailsDto
                    {
                        Id = cd.Id,
                        ProductId = cd.ProductId,
                        ProductName = cd.ProductName,
                        Count = cd.Count,
                        UnitPrice = cd.UnitPrice,
                        CardHeaderId = cd.CardHeaderId
                    }).ToList() ?? new List<CartDetailsDto>()
                }).ToList();
            });
        return m.Object;
    }

    public static IMapper ForCartHeaderGetById()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<CartHeaderDto>(It.IsAny<CartHeader>()))
            .Returns((CartHeader c) => new CartHeaderDto
            {
                Id = c.Id,
                UserId = c.UserId,
                TotalPrice = c.TotalPrice,
                CartDetails = c.CartDetails?.Select(cd => new CartDetailsDto
                {
                    Id = cd.Id,
                    ProductId = cd.ProductId,
                    ProductName = cd.ProductName,
                    Count = cd.Count,
                    UnitPrice = cd.UnitPrice,
                    CardHeaderId = cd.CardHeaderId
                }).ToList() ?? new List<CartDetailsDto>()
            });
        return m.Object;
    }

    public static IMapper ForCheckout()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<BasketCheckoutEvent>(It.IsAny<BasketCheckout>()))
            .Returns((BasketCheckout b) => new BasketCheckoutEvent
            {
                UserName = b.UserName,
                TotalPrice = (double)b.TotalPrice,
                FirstName = b.FirstName,
                LastName = b.LastName,
                EmailAddress = b.EmailAddress,
                AddressLine = b.AddressLine,
                Country = b.Country,
                City = b.City,
                ZipCode = b.ZipCode,
                CardName = b.CardName,
                CardNumber = b.CardNumber,
                Expiration = b.Expiration,
                CVV = b.CVV,
                PaymentMethod = b.PaymentMethod
            });
        return m.Object;
    }
}
