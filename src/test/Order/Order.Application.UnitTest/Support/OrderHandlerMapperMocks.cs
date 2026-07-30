using AutoMapper;
using Moq;
using Order.Application.DTOs.Order;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Domain.Entity;

namespace Order.Application.UnitTest.Support;

internal static class OrderHandlerMapperMocks
{
    public static IMapper ForCheckout()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<ProductOrder>(It.IsAny<CheckoutOrderCommand>()))
            .Returns((CheckoutOrderCommand c) => new ProductOrder
            {
                UserName = c.UserName,
                TotalPrice = c.TotalPrice,
                FirstName = c.FirstName,
                LastName = c.LastName,
                EmailAddress = c.EmailAddress,
                AddressLine = c.AddressLine,
                Country = c.Country,
                City = c.City,
                ZipCode = c.ZipCode,
                CardName = c.CardName,
                CardNumber = c.CardNumber,
                Expiration = c.Expiration,
                CVV = c.CVV,
                PaymentMethod = c.PaymentMethod
            });
        return m.Object;
    }

    public static IMapper ForUpdate()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
            .Callback((object src, object dest, Type _, Type __) =>
            {
                if (src is UpdateOrderCommand cmd && dest is ProductOrder po)
                {
                    po.UserName = cmd.UserName;
                    po.TotalPrice = cmd.TotalPrice;
                    po.FirstName = cmd.FirstName;
                    po.LastName = cmd.LastName;
                    po.EmailAddress = cmd.EmailAddress;
                    po.AddressLine = cmd.AddressLine;
                    po.Country = cmd.Country;
                    po.City = cmd.City;
                    po.ZipCode = cmd.ZipCode;
                    po.CardName = cmd.CardName;
                    po.CardNumber = cmd.CardNumber;
                    po.Expiration = cmd.Expiration;
                    po.CVV = cmd.CVV;
                    po.PaymentMethod = cmd.PaymentMethod;
                }
            });
        return m.Object;
    }

    public static IMapper ForOrderList()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<List<OrderVm>>(It.IsAny<object>()))
            .Returns((object src) =>
            {
                if (src is not IEnumerable<ProductOrder> list)
                    return new List<OrderVm>();
                return list.Select(o => new OrderVm
                {
                    Id = o.Id,
                    UserName = o.UserName,
                    TotalPrice = o.TotalPrice,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    EmailAddress = o.EmailAddress,
                    AddressLine = o.AddressLine,
                    Country = o.Country,
                    City = o.City,
                    ZipCode = o.ZipCode,
                    CardName = o.CardName,
                    CardNumber = o.CardNumber,
                    Expiration = o.Expiration,
                    CVV = o.CVV,
                    PaymentMethod = o.PaymentMethod
                }).ToList();
            });
        return m.Object;
    }

    public static IMapper Unused()
    {
        return new Mock<IMapper>().Object;
    }
}
