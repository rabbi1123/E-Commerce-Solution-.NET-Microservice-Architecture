using System.Security.Cryptography;
using System.Text;
using IdentityWebAPI.Data;
using IdentityWebAPI.Models;
using IdentityWebAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityWebAPI.Services;

public class EmailVerificationTokenService : IEmailVerificationTokenService
{
    private const string InvalidTokenMessage = "This verification link is invalid or has expired.";

    private readonly IdentityDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentitySettings _settings;

    public EmailVerificationTokenService(
        IdentityDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IOptions<IdentitySettings> settings)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _settings = settings.Value;
    }

    public async Task<string> IssueAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        var unused = await _dbContext.EmailVerificationTokens
            .Where(token => token.UserId == userId && token.UsedOn == null)
            .ToListAsync();

        foreach (var token in unused)
        {
            token.UsedOn = now;
        }

        var rawToken = CreateRawToken();
        _dbContext.EmailVerificationTokens.Add(new EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = HashToken(rawToken),
            ExpiresOn = now.AddMinutes(_settings.EmailVerificationTokenMinutes),
            CreatedOn = now
        });

        await _dbContext.SaveChangesAsync();
        return rawToken;
    }

    public async Task<AuthOperationResult> VerifyAsync(string? rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, InvalidTokenMessage);
        }

        var tokenHash = HashToken(rawToken.Trim());
        var stored = await _dbContext.EmailVerificationTokens
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash);

        if (stored is null || stored.UsedOn is not null || stored.ExpiresOn <= DateTime.UtcNow)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, InvalidTokenMessage);
        }

        var user = await _userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, InvalidTokenMessage);
        }

        var now = DateTime.UtcNow;
        user.EmailConfirmed = true;
        stored.UsedOn = now;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, result.Errors);
        }

        return AuthOperationResult.Success(message: "Email address verified.");
    }

    private static string CreateRawToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string HashToken(string rawToken)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
