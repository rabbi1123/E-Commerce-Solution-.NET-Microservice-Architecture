using IdentityWebAPI.Controllers;
using IdentityWebAPI.DTOs;
using IdentityWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityWebAPI.UnitTest.Controllers;

public class AccountControllerTests
{
    private static AuthResponse SampleAuthResponse() => new()
    {
        AccessToken = "access-token",
        RefreshToken = "refresh-token"
    };

    [Fact]
    public async Task Register_WhenSucceeded_ReturnsOkWithAuthResponse()
    {
        var request = new RegisterRequest
        {
            Email = "user@example.com",
            Password = "Password1!",
            Role = "Customer"
        };
        var response = SampleAuthResponse();
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.RegisterAsync(request))
            .ReturnsAsync(AuthOperationResult.Success(response));

        var controller = new AccountController(mockService.Object);

        var result = await controller.Register(request);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(response, ok.Value);
        mockService.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Register_WhenConflict_ReturnsConflict()
    {
        var request = new RegisterRequest
        {
            Email = "exists@example.com",
            Password = "Password1!",
            Role = "Customer"
        };
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.RegisterAsync(request))
            .ReturnsAsync(AuthOperationResult.Fail(AuthErrorType.Conflict, "Email already registered."));

        var controller = new AccountController(mockService.Object);

        var result = await controller.Register(request);

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
        mockService.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Login_WhenSucceeded_ReturnsOkWithAuthResponse()
    {
        var request = new LoginRequest
        {
            Email = "user@example.com",
            Password = "Password1!"
        };
        var response = SampleAuthResponse();
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.LoginAsync(request))
            .ReturnsAsync(AuthOperationResult.Success(response));

        var controller = new AccountController(mockService.Object);

        var result = await controller.Login(request);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(response, ok.Value);
        mockService.Verify(s => s.LoginAsync(request), Times.Once);
    }

    [Fact]
    public async Task Login_WhenUnauthorized_ReturnsUnauthorized()
    {
        var request = new LoginRequest
        {
            Email = "user@example.com",
            Password = "wrong"
        };
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.LoginAsync(request))
            .ReturnsAsync(AuthOperationResult.Fail(AuthErrorType.Unauthorized, "Invalid credentials."));

        var controller = new AccountController(mockService.Object);

        var result = await controller.Login(request);

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
        mockService.Verify(s => s.LoginAsync(request), Times.Once);
    }

    [Fact]
    public async Task Refresh_WhenSucceeded_ReturnsOkWithAuthResponse()
    {
        var request = new RefreshRequest
        {
            AccessToken = "old-access",
            RefreshToken = "old-refresh"
        };
        var response = SampleAuthResponse();
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.RefreshAsync(request))
            .ReturnsAsync(AuthOperationResult.Success(response));

        var controller = new AccountController(mockService.Object);

        var result = await controller.Refresh(request);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(response, ok.Value);
        mockService.Verify(s => s.RefreshAsync(request), Times.Once);
    }

    [Fact]
    public async Task Logout_CallsServiceAndReturnsOk()
    {
        var request = new LogoutRequest { RefreshToken = "refresh-token" };
        var mockService = new Mock<IAccountService>();
        mockService.Setup(s => s.LogoutAsync(request)).Returns(Task.CompletedTask);

        var controller = new AccountController(mockService.Object);

        var result = await controller.Logout(request);

        Assert.IsType<OkObjectResult>(result);
        mockService.Verify(s => s.LogoutAsync(request), Times.Once);
    }

    [Fact]
    public async Task ForgotPassword_CallsServiceAndReturnsOk()
    {
        var request = new ForgotPasswordRequest { Email = "user@example.com" };
        var mockService = new Mock<IAccountService>();
        mockService.Setup(s => s.ForgotPasswordAsync(request)).Returns(Task.CompletedTask);

        var controller = new AccountController(mockService.Object);

        var result = await controller.ForgotPassword(request);

        Assert.IsType<OkObjectResult>(result);
        mockService.Verify(s => s.ForgotPasswordAsync(request), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WhenSucceeded_ReturnsOkWithMessage()
    {
        var request = new ResetPasswordRequest
        {
            Email = "user@example.com",
            Token = "reset-token",
            NewPassword = "NewPassword1!"
        };
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.ResetPasswordAsync(request))
            .ReturnsAsync(AuthOperationResult.Success(message: "Password reset successfully."));

        var controller = new AccountController(mockService.Object);

        var result = await controller.ResetPassword(request);

        Assert.IsType<OkObjectResult>(result);
        mockService.Verify(s => s.ResetPasswordAsync(request), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WhenBadRequestWithErrors_ReturnsBadRequest()
    {
        var request = new ResetPasswordRequest
        {
            Email = "user@example.com",
            Token = "bad-token",
            NewPassword = "short"
        };
        var errors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Password is too short." }
        };
        var mockService = new Mock<IAccountService>();
        mockService
            .Setup(s => s.ResetPasswordAsync(request))
            .ReturnsAsync(AuthOperationResult.Fail(AuthErrorType.BadRequest, errors));

        var controller = new AccountController(mockService.Object);

        var result = await controller.ResetPassword(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Same(errors, badRequest.Value);
        mockService.Verify(s => s.ResetPasswordAsync(request), Times.Once);
    }
}
