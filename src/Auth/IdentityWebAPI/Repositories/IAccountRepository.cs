using IdentityWebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Repositories;

public interface IAccountRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);

    Task<ApplicationUser?> FindByIdAsync(Guid userId);

    Task<bool> RoleExistsAsync(string roleName);

    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}
