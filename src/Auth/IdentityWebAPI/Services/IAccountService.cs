using IdentityWebAPI.DTOs;

namespace IdentityWebAPI.Services;

public interface IAccountService
{
    Task<AuthOperationResult> RegisterAsync(RegisterRequest request);

    Task<AuthOperationResult> LoginAsync(LoginRequest request);

    Task<AuthOperationResult> RefreshAsync(RefreshRequest request);

    Task LogoutAsync(LogoutRequest request);

    Task ForgotPasswordAsync(ForgotPasswordRequest request);

    Task<AuthOperationResult> ResetPasswordAsync(ResetPasswordRequest request);

    Task<AuthOperationResult> VerifyEmailAsync(string? token);

    Task ResendVerificationAsync(string email);
}
