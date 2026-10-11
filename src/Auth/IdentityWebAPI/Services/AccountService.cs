using System.Text;
using EventBus.RabbitMQ;
using Eventbus.Messages.Events;
using IdentityWebAPI.DTOs;
using IdentityWebAPI.Models;
using IdentityWebAPI.Repositories;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityWebAPI.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;
    private readonly IEventBus _eventBus;
    private readonly IEmailVerificationTokenService _emailVerificationTokens;

    public AccountService(
        IAccountRepository accountRepository,
        ITokenService tokenService,
        IEventBus eventBus,
        IEmailVerificationTokenService emailVerificationTokens)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _eventBus = eventBus;
        _emailVerificationTokens = emailVerificationTokens;
    }

    public async Task<AuthOperationResult> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _accountRepository.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return AuthOperationResult.Fail(AuthErrorType.Conflict, "A user with this email already exists.");
        }

        if (!await _accountRepository.RoleExistsAsync(request.Role))
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, $"Role '{request.Role}' does not exist.");
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false
        };

        var result = await _accountRepository.CreateUserAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, result.Errors);
        }

        await _accountRepository.AddToRoleAsync(user, request.Role);

        await _eventBus.PublishAsync(new UserRegistrationEvent
        {
            UserId = user.Id,
            Email = user.Email!
        });

        await PublishVerificationEmailAsync(user);

        return AuthOperationResult.Success(message: "Registration successful. Please verify your email address.");
    }

    public async Task<AuthOperationResult> LoginAsync(LoginRequest request)
    {
        var user = await _accountRepository.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return AuthOperationResult.Fail(AuthErrorType.Unauthorized, "Invalid email or password.");
        }

        var isValidPassword = await _accountRepository.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            return AuthOperationResult.Fail(AuthErrorType.Unauthorized, "Invalid email or password.");
        }

        if (!user.EmailConfirmed)
        {
            return AuthOperationResult.Fail(
                AuthErrorType.Unauthorized,
                "Please verify your email address before logging in.");
        }

        var tokens = await _tokenService.CreateTokenPairAsync(user);
        return AuthOperationResult.Success(tokens);
    }

    public async Task<AuthOperationResult> RefreshAsync(RefreshRequest request)
    {
        var tokens = await _tokenService.RefreshTokenPairAsync(request.AccessToken, request.RefreshToken);
        if (tokens is null)
        {
            return AuthOperationResult.Fail(AuthErrorType.Unauthorized, "Invalid or expired refresh token.");
        }

        return AuthOperationResult.Success(tokens);
    }

    public Task LogoutAsync(LogoutRequest request)
        => _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _accountRepository.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return;
        }
    }

    public async Task<AuthOperationResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _accountRepository.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, "Invalid password reset request.");
        }

        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        }
        catch
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, "Invalid password reset token.");
        }

        var result = await _accountRepository.ResetPasswordAsync(user, decodedToken, request.NewPassword);
        if (!result.Succeeded)
        {
            return AuthOperationResult.Fail(AuthErrorType.BadRequest, result.Errors);
        }

        await _tokenService.RevokeAllRefreshTokensAsync(user.Id);
        return AuthOperationResult.Success(message: "Password has been reset successfully.");
    }

    public Task<AuthOperationResult> VerifyEmailAsync(string? token)
        => _emailVerificationTokens.VerifyAsync(token);

    public async Task ResendVerificationAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        var user = await _accountRepository.FindByEmailAsync(email.Trim());
        if (user is null || user.EmailConfirmed || string.IsNullOrWhiteSpace(user.Email))
        {
            return;
        }

        await PublishVerificationEmailAsync(user);
    }

    private async Task PublishVerificationEmailAsync(ApplicationUser user)
    {
        var verificationToken = await _emailVerificationTokens.IssueAsync(user.Id);
        await _eventBus.PublishAsync(new UserVerificationEmailEvent
        {
            UserId = user.Id,
            Email = user.Email!,
            VerificationToken = verificationToken
        });
    }
}
