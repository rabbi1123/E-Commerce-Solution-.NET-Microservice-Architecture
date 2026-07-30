using IdentityWebAPI.DTOs;
using IdentityWebAPI.Models;

namespace IdentityWebAPI.Services;

public interface ITokenService
{
    Task<AuthResponse> CreateTokenPairAsync(ApplicationUser user);

    Task<AuthResponse?> RefreshTokenPairAsync(string accessToken, string refreshToken);

    Task RevokeRefreshTokenAsync(string refreshToken);

    Task RevokeAllRefreshTokensAsync(Guid userId);
}
