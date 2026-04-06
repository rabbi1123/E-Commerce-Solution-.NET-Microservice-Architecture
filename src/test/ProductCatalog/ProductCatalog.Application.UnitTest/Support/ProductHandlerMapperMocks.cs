using AutoMapper;
using ECommerce.Application.DTOs.Product;
using ECommerce.Domain.Entity;
using Moq;

namespace ProductCatalog.Application.UnitTest.Support;

internal static class ProductHandlerMapperMocks
{
    public static IMapper ForAdd()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<Product>(It.IsAny<ProductCreateDto>()))
            .Returns((ProductCreateDto d) => new Product
            {
                name = d.name,
                price = d.price,
                description = d.description,
                categoryName = d.categoryName,
                imageUrl = d.imageUrl
            });
        return m.Object;
    }

    public static IMapper ForUpdate()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<Product>(It.IsAny<ProductDto>()))
            .Returns((ProductDto d) => new Product
            {
                Id = d.Id,
                name = d.name,
                price = d.price,
                description = d.description,
                categoryName = d.categoryName,
                imageUrl = d.imageUrl
            });
        return m.Object;
    }

    public static IMapper ForList()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<List<ProductDto>>(It.IsAny<object>()))
            .Returns((object src) =>
            {
                if (src is not IEnumerable<Product> list)
                    return new List<ProductDto>();
                return list.Select(p => new ProductDto
                {
                    Id = p.Id,
                    name = p.name,
                    price = p.price,
                    description = p.description,
                    categoryName = p.categoryName,
                    imageUrl = p.imageUrl
                }).ToList();
            });
        return m.Object;
    }

    public static IMapper ForGetById()
    {
        var m = new Mock<IMapper>();
        m.Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
            .Returns((Product p) => new ProductDto
            {
                Id = p.Id,
                name = p.name,
                price = p.price,
                description = p.description,
                categoryName = p.categoryName,
                imageUrl = p.imageUrl
            });
        return m.Object;
    }

    public static IMapper Unused() => new Mock<IMapper>().Object;
}
