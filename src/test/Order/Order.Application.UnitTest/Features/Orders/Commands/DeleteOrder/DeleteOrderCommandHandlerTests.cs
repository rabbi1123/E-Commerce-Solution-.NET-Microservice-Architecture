using Moq;
using Order.Application.Features.Orders.Commands.DeleteOrder;
using Order.Application.Contracts.Persistence;
using Order.Application.Exceptions;
using Order.Application.UnitTest.Support;
using Order.Domain.Entity;

namespace Order.Application.UnitTest.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenOrderExists_DeletesFromRepository()
    {
        var order = new ProductOrder { Id = 5, UserName = "u" };
        var repo = new Mock<IOrderRepository>();
        repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(order);

        var sut = new DeleteOrderCommandHandler(repo.Object, OrderHandlerMapperMocks.Unused());

        await sut.Handle(new DeleteOrderCommand(5), CancellationToken.None);

        repo.Verify(r => r.DeleteAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOrderMissing_ThrowsNotFoundException()
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(r => r.GetByIdAsync(99))
            .Returns(Task.FromResult<ProductOrder>(null!));

        var sut = new DeleteOrderCommandHandler(repo.Object, OrderHandlerMapperMocks.Unused());

        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            sut.Handle(new DeleteOrderCommand(99), CancellationToken.None));

        Assert.Contains("ProductOrder", ex.Message);
        Assert.Contains("99", ex.Message);
        repo.Verify(r => r.DeleteAsync(It.IsAny<ProductOrder>()), Times.Never);
    }
}
