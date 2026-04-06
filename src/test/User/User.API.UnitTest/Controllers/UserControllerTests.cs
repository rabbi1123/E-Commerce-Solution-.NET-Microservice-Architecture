using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using User.API.Controllers;
using User.API.Models;
using User.API.Services;

namespace User.API.UnitTest.Controllers;

public class UserControllerTests
{
    private static ApplicationUser SampleUser(int id = 1) => new()
    {
        Id = id,
        UserName = "testuser",
        Email = "test@example.com",
        Name = "Test",
        CreatedBy = 1,
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetAll_CallsServiceAndReturnsOkWithUsers()
    {
        var users = new List<ApplicationUser> { SampleUser(1), SampleUser(2) };
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
        var user = SampleUser(5);
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var controller = new UserController(mockService.Object);

        var result = await controller.GetById(user.Id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(user, ok.Value);
        mockService.Verify(s => s.GetByIdAsync(user.Id), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenUserMissing_ReturnsNotFound()
    {
        const int id = 99;
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ApplicationUser?)null);

        var controller = new UserController(mockService.Object);

        var result = await controller.GetById(id);

        Assert.IsType<NotFoundResult>(result.Result);
        mockService.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task Create_CallsServiceAndReturnsCreatedAtAction()
    {
        var input = SampleUser(0);
        var created = SampleUser(3);
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.CreateAsync(input)).ReturnsAsync(created);

        var controller = new UserController(mockService.Object);

        var result = await controller.Create(input);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(nameof(UserController.GetById), createdResult.ActionName);
        Assert.True(createdResult.RouteValues!.TryGetValue("id", out var routeId));
        Assert.Equal(created.Id, Assert.IsType<int>(routeId));
        Assert.Same(created, createdResult.Value);
        mockService.Verify(s => s.CreateAsync(input), Times.Once);
    }

    [Fact]
    public async Task Update_WhenUserExists_ReturnsOkWithUser()
    {
        const int id = 4;
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
        const int id = 100;
        var payload = SampleUser(id);
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateAsync(id, payload)).ReturnsAsync((ApplicationUser?)null);

        var controller = new UserController(mockService.Object);

        var result = await controller.Update(id, payload);

        Assert.IsType<NotFoundResult>(result.Result);
        mockService.Verify(s => s.UpdateAsync(id, payload), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenDeleted_ReturnsNoContent()
    {
        const int id = 6;
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
        const int id = 404;
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var controller = new UserController(mockService.Object);

        var result = await controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
        mockService.Verify(s => s.DeleteAsync(id), Times.Once);
    }
}
