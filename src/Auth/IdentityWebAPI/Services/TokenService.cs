using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityWebAPI.Data;
using IdentityWebAPI.DTOs;
using IdentityWebAPI.Models;
using IdentityWebAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityWebAPI.Services;

public class TokenService : ITokenService
{
    public const string PermissionClaimType = "permission";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IdentityDbContext _dbContext;
    private readonly JwtSettings _jwtSettings;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IdentityDbContext dbContext,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> CreateTokenPairAsync(ApplicationUser user)
    {
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = await PersistRefreshTokenAsync(user.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse?> RefreshTokenPairAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal is null)
        {
            return null;
        }

        var userIdValue = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken && x.UserId == userId);

        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpireDate <= DateTime.UtcNow)
        {
            return null;
        }

        storedToken.IsRevoked = true;
        await _dbContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return null;
        }

        return await CreateTokenPairAsync(user);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (storedToken is null || storedToken.IsRevoked)
        {
            return;
        }

        storedToken.IsRevoked = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task RevokeAllRefreshTokensAsync(Guid userId)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        if (tokens.Count > 0)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));

            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole is null)
            {
                continue;
            }

            var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
            foreach (var roleClaim in roleClaims.Where(c => c.Type == PermissionClaimType))
            {
                claims.Add(new Claim(PermissionClaimType, roleClaim.Value));
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> PersistRefreshTokenAsync(Guid userId)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshToken,
            ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            IsRevoked = false
        });

        await _dbContext.SaveChangesAsync();
        return refreshToken;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
