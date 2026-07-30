using IdentityWebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public Task<ApplicationUser?> FindByEmailAsync(string email)
        => _userManager.FindByEmailAsync(email);

    public Task<ApplicationUser?> FindByIdAsync(Guid userId)
        => _userManager.FindByIdAsync(userId.ToString());

    public Task<bool> RoleExistsAsync(string roleName)
        => _roleManager.RoleExistsAsync(roleName);

    public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        => _userManager.CreateAsync(user, password);

    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
        => _userManager.AddToRoleAsync(user, roleName);

    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => _userManager.CheckPasswordAsync(user, password);

    public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        => _userManager.GeneratePasswordResetTokenAsync(user);

    public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        => _userManager.ResetPasswordAsync(user, token, newPassword);
}
