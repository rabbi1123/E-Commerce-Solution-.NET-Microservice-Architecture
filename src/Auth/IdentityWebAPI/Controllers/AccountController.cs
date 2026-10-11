using IdentityWebAPI.DTOs;
using IdentityWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace IdentityWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var result = await _accountService.RegisterAsync(request);
        return ToActionResult(result);
    }

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _accountService.LoginAsync(request);
        return ToActionResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _accountService.RefreshAsync(request);
        return ToActionResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await _accountService.LogoutAsync(request);
        return Ok(new { message = "Logged out successfully." });
    }

    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _accountService.ForgotPasswordAsync(request);
        return Ok(new { message = "If an account with that email exists, a password reset message has been sent." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _accountService.ResetPasswordAsync(request);
        return ToActionResult(result);
    }

    [HttpGet("verify-email")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string? token)
    {
        var result = await _accountService.VerifyEmailAsync(token);
        return ToActionResult(result);
    }

    [HttpPost("resend-verification")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequest request)
    {
        await _accountService.ResendVerificationAsync(request.Email);
        return Ok(new { message = "If an account with that email exists and is not yet verified, a verification link has been sent." });
    }

    private ActionResult ToActionResult(AuthOperationResult result)
    {
        if (result.Succeeded)
        {
            if (result.Data is not null)
            {
                return Ok(result.Data);
            }

            return Ok(new { message = result.Message });
        }

        return result.ErrorType switch
        {
            AuthErrorType.Conflict => Conflict(new { message = result.Message }),
            AuthErrorType.Unauthorized => Unauthorized(new { message = result.Message }),
            AuthErrorType.BadRequest when result.Errors is not null => BadRequest(result.Errors),
            _ => BadRequest(new { message = result.Message })
        };
    }
}
