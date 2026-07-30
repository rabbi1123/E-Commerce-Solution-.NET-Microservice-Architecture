using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using User.API.Controllers;
using User.API.Services;
using UserEntity = User.API.Models.User;

namespace User.API.UnitTest.Controllers;

public class UserControllerTests
{
    private static UserEntity SampleUser(Guid? userId = null) => new()
    {
        UserId = userId ?? Guid.Parse("11111111-1111-1111-1111-111111111111"),
        FirstName = "Test",
        LastName = "User",
        Phone = "1234567890",
        Address = "123 Main St",
        Photo = "photo.jpg",
        Gender = "Male",
        BirthDate = new DateTime(1990, 1, 1)
    };

    [Fact]
    public async Task GetAll_CallsServiceAndReturnsOkWithUsers()
    {
        var users = new List<UserEntity>
        {
            SampleUser(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            SampleUser(Guid.Parse("22222222-2222-2222-2222-222222222222"))
        };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        var controller = new UserController(mockService.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(users, ok.Value);
        mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenUserExists_ReturnsOkWithUser()
    {
        var user = SampleUser();
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetByIdAsync(user.UserId)).ReturnsAsync(user);

        var controller = new UserController(mockService.Object);

        var result = await controller.GetById(user.UserId);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(user, ok.Value);
        mockService.Verify(s => s.GetByIdAsync(user.UserId), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenUserMissing_ReturnsNotFound()
    {
        var id = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((UserEntity?)null);

        var controller = new UserController(mockService.Object);

        var result = await controller.GetById(id);

        Assert.IsType<NotFoundResult>(result.Result);
        mockService.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task Create_CallsServiceAndReturnsCreatedAtAction()
    {
        var input = SampleUser();
        var created = SampleUser();
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.CreateAsync(input)).ReturnsAsync(created);

        var controller = new UserController(mockService.Object);

        var result = await controller.Create(input);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(nameof(UserController.GetById), createdResult.ActionName);
        Assert.True(createdResult.RouteValues!.TryGetValue("userId", out var routeId));
        Assert.Equal(created.UserId, Assert.IsType<Guid>(routeId));
        Assert.Same(created, createdResult.Value);
        mockService.Verify(s => s.CreateAsync(input), Times.Once);
    }

    [Fact]
    public async Task Create_WhenUserIdEmpty_ReturnsBadRequest()
    {
        var input = SampleUser(Guid.Empty);
        var mockService = new Mock<IUserService>();

        var controller = new UserController(mockService.Object);

        var result = await controller.Create(input);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("UserId is required.", badRequest.Value);
        mockService.Verify(s => s.CreateAsync(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenUserExists_ReturnsOkWithUser()
    {
        var id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var payload = SampleUser(id);
        var updated = SampleUser(id);
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateAsync(id, payload)).ReturnsAsync(updated);

        var controller = new UserController(mockService.Object);

        var result = await controller.Update(id, payload);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(updated, ok.Value);
        mockService.Verify(s => s.UpdateAsync(id, payload), Times.Once);
    }

    [Fact]
    public async Task Update_WhenUserMissing_ReturnsNotFound()
    {
        var id = Guid.Parse("10000000-0000-0000-0000-000000000100");
        var payload = SampleUser(id);
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateAsync(id, payload)).ReturnsAsync((UserEntity?)null);

        var controller = new UserController(mockService.Object);

        var result = await controller.Update(id, payload);

        Assert.IsType<NotFoundResult>(result.Result);
        mockService.Verify(s => s.UpdateAsync(id, payload), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenDeleted_ReturnsNoContent()
    {
        var id = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var controller = new UserController(mockService.Object);

        var result = await controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        mockService.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        var id = Guid.Parse("40440440-4044-4044-4044-404404404404");
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var controller = new UserController(mockService.Object);

        var result = await controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
        mockService.Verify(s => s.DeleteAsync(id), Times.Once);
    }
}
