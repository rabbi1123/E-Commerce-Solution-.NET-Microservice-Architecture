using AutoMapper;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;
using Grpc.Core;
using Moq;

namespace Discount.Grpc.UnitTest.Services;

public class DiscountServiceTests
{
    private static ProductDiscount SampleCoupon(int productId = 99) => new()
    {
        Id = 1,
        ProductId = productId,
        ProductName = "Widget",
        Description = "Sample",
        Amount = 12.5
    };

    [Fact]
    public async Task GetDiscount_WhenFound_ReturnsMappedModel()
    {
        const int productId = 42;
        var coupon = SampleCoupon(productId);
        var expectedModel = new DiscountModel
        {
            Id = coupon.Id,
            ProductId = coupon.ProductId,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };

        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.GetDiscount(productId)).ReturnsAsync(coupon);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DiscountModel>(coupon)).Returns(expectedModel);

        var service = new DiscountService(mockRepo.Object, mockMapper.Object);
        var request = new GetDiscountRequest { ProductId = productId };

        var result = await service.GetDiscount(request, CreateCallContext());

        Assert.Same(expectedModel, result);
        mockRepo.Verify(r => r.GetDiscount(productId), Times.Once);
        mockMapper.Verify(m => m.Map<DiscountModel>(coupon), Times.Once);
    }

    [Fact]
    public async Task GetDiscount_WhenNotFound_ThrowsRpcNotFound()
    {
        const int productId = 404;
        var mockRepo = new Mock<IDiscountRepository>();
        mockRepo.Setup(r => r.GetDiscount(productId)).ReturnsAsync((ProductDiscount?)null);

        var mockMapper = new Mock<IMapper>();
        var service = new DiscountService(mockRepo.Object, mockMapper.Object);
        var request = new GetDiscountRequest { ProductId = productId };

        var ex = await Assert.ThrowsAsync<RpcException>(() =>
            service.GetDiscount(request, CreateCallContext()));

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
        Assert.Contains($"{productId}", ex.Status.Detail, StringComparison.Ordinal);
        mockMapper.Verify(m => m.Map<DiscountModel>(It.IsAny<ProductDiscount>()), Times.Never);
    }

    private static ServerCallContext CreateCallContext() =>
        Mock.Of<ServerCallContext>();
}
