using Discount.API.Controllers;
using Discount.API.Models;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Discount.API.UnitTest.Controllers;

public class DiscountControllerTests
{
    private static ProductDiscount SampleDiscount(int productId = 42) => new()
    {
        Id = 1,
        ProductId = productId,
        ProductName = "Test product",
        Description = "Test description",
        Amount = 10.5
    };

    [Fact]
    public async Task Get_CallsRepositoryAndReturnsOkWithDiscount()
    {
        var discount = SampleDiscount();
        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.GetDiscount(discount.ProductId)).ReturnsAsync(discount);

        var controller = new DiscountController(mockRepo.Object);

        var result = await controller.Get(discount.ProductId);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(discount, ok.Value);
        mockRepo.Verify(r => r.GetDiscount(discount.ProductId), Times.Once);
    }

    [Fact]
    public async Task Create_CallsRepositoryAndReturnsCreatedAtRoute()
    {
        var discount = SampleDiscount();
        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.CreateDiscount(discount)).ReturnsAsync(true);

        var controller = new DiscountController(mockRepo.Object);

        var result = await controller.Create(discount);

        var created = Assert.IsType<CreatedAtRouteResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal("GetDiscount", created.RouteName);
        Assert.True(created.RouteValues!.TryGetValue("productId", out var routeProductId));
        Assert.Equal(discount.ProductId, Assert.IsType<int>(routeProductId));
        Assert.Same(discount, created.Value);
        mockRepo.Verify(r => r.CreateDiscount(discount), Times.Once);
    }

    [Fact]
    public async Task Update_CallsRepositoryAndReturnsOkWithResult()
    {
        var discount = SampleDiscount();
        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.UpdateDiscount(discount)).ReturnsAsync(true);

        var controller = new DiscountController(mockRepo.Object);

        var result = await controller.Update(discount);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True((bool)(ok.Value ?? false));
        mockRepo.Verify(r => r.UpdateDiscount(discount), Times.Once);
    }

    [Fact]
    public async Task Delete_CallsRepositoryAndReturnsOkWithResult()
    {
        const int productId = 7;
        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.DeleteDiscount(productId)).ReturnsAsync(true);

        var controller = new DiscountController(mockRepo.Object);

        var result = await controller.Delete(productId);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True((bool)(ok.Value ?? false));
        mockRepo.Verify(r => r.DeleteDiscount(productId), Times.Once);
    }
}
