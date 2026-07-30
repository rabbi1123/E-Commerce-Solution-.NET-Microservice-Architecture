using System.Security.Claims;
using IdentityWebAPI.Models;
using IdentityWebAPI.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Data;

public static class IdentitySeed
{
    private static readonly string[] DefaultPermissions =
    [
        "Product.Read",
        "Product.Create",
        "Product.Update",
        "Product.Delete"
    ];

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        await EnsureRoleAsync(roleManager, "Admin", DefaultPermissions);
        await EnsureRoleAsync(roleManager, "User", ["Product.Read"]);
    }

    private static async Task EnsureRoleAsync(
        RoleManager<ApplicationRole> roleManager,
        string roleName,
        IEnumerable<string> permissions)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            role = new ApplicationRole(roleName)
            {
                Id = Guid.NewGuid()
            };

            await roleManager.CreateAsync(role);
        }

        var existingClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (existingClaims.Any(c => c.Type == TokenService.PermissionClaimType && c.Value == permission))
            {
                continue;
            }

            await roleManager.AddClaimAsync(role, new Claim(TokenService.PermissionClaimType, permission));
        }
    }
}
