using IdentityWebAPI.Models;
using IdentityWebAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityWebAPI.UnitTest.Repositories;

public class AccountRepositoryTests
{
    private static ApplicationUser SampleUser(Guid? id = null) => new()
    {
        Id = id ?? Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Email = "user@example.com",
        UserName = "user@example.com"
    };

    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }

    private static Mock<RoleManager<ApplicationRole>> CreateRoleManagerMock()
    {
        var store = new Mock<IRoleStore<ApplicationRole>>();
        return new Mock<RoleManager<ApplicationRole>>(
            store.Object,
            null!,
            null!,
            null!,
            null!);
    }

    [Fact]
    public async Task FindByEmailAsync_CallsUserManagerAndReturnsUser()
    {
        var user = SampleUser();
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.FindByEmailAsync(user.Email!);

        Assert.Same(user, result);
        userManager.Verify(m => m.FindByEmailAsync(user.Email!), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_CallsUserManagerAndReturnsUser()
    {
        var user = SampleUser();
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.FindByIdAsync(user.Id);

        Assert.Same(user, result);
        userManager.Verify(m => m.FindByIdAsync(user.Id.ToString()), Times.Once);
    }

    [Fact]
    public async Task RoleExistsAsync_CallsRoleManagerAndReturnsResult()
    {
        var roleManager = CreateRoleManagerMock();
        roleManager.Setup(m => m.RoleExistsAsync("Customer")).ReturnsAsync(true);
        var repository = new AccountRepository(CreateUserManagerMock().Object, roleManager.Object);

        var result = await repository.RoleExistsAsync("Customer");

        Assert.True(result);
        roleManager.Verify(m => m.RoleExistsAsync("Customer"), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_CallsUserManagerAndReturnsIdentityResult()
    {
        var user = SampleUser();
        const string password = "Password1!";
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.CreateAsync(user, password)).ReturnsAsync(IdentityResult.Success);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.CreateUserAsync(user, password);

        Assert.True(result.Succeeded);
        userManager.Verify(m => m.CreateAsync(user, password), Times.Once);
    }

    [Fact]
    public async Task AddToRoleAsync_CallsUserManagerAndReturnsIdentityResult()
    {
        var user = SampleUser();
        const string role = "Customer";
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.AddToRoleAsync(user, role)).ReturnsAsync(IdentityResult.Success);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.AddToRoleAsync(user, role);

        Assert.True(result.Succeeded);
        userManager.Verify(m => m.AddToRoleAsync(user, role), Times.Once);
    }

    [Fact]
    public async Task CheckPasswordAsync_CallsUserManagerAndReturnsResult()
    {
        var user = SampleUser();
        const string password = "Password1!";
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.CheckPasswordAsync(user, password)).ReturnsAsync(true);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.CheckPasswordAsync(user, password);

        Assert.True(result);
        userManager.Verify(m => m.CheckPasswordAsync(user, password), Times.Once);
    }

    [Fact]
    public async Task GeneratePasswordResetTokenAsync_CallsUserManagerAndReturnsToken()
    {
        var user = SampleUser();
        const string token = "reset-token";
        var userManager = CreateUserManagerMock();
        userManager.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.GeneratePasswordResetTokenAsync(user);

        Assert.Equal(token, result);
        userManager.Verify(m => m.GeneratePasswordResetTokenAsync(user), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordAsync_CallsUserManagerAndReturnsIdentityResult()
    {
        var user = SampleUser();
        const string token = "reset-token";
        const string newPassword = "NewPassword1!";
        var userManager = CreateUserManagerMock();
        userManager
            .Setup(m => m.ResetPasswordAsync(user, token, newPassword))
            .ReturnsAsync(IdentityResult.Success);
        var repository = new AccountRepository(userManager.Object, CreateRoleManagerMock().Object);

        var result = await repository.ResetPasswordAsync(user, token, newPassword);

        Assert.True(result.Succeeded);
        userManager.Verify(m => m.ResetPasswordAsync(user, token, newPassword), Times.Once);
    }
}
