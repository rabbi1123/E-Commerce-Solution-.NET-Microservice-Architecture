using IdentityWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Services;

public class AuthOperationResult
{
    public bool Succeeded { get; private set; }

    public AuthResponse? Data { get; private set; }

    public string? Message { get; private set; }

    public IEnumerable<IdentityError>? Errors { get; private set; }

    public AuthErrorType ErrorType { get; private set; }

    public static AuthOperationResult Success(AuthResponse? data = null, string? message = null)
        => new()
        {
            Succeeded = true,
            Data = data,
            Message = message,
            ErrorType = AuthErrorType.None
        };

    public static AuthOperationResult Fail(AuthErrorType errorType, string message)
        => new()
        {
            Succeeded = false,
            Message = message,
            ErrorType = errorType
        };

    public static AuthOperationResult Fail(AuthErrorType errorType, IEnumerable<IdentityError> errors)
        => new()
        {
            Succeeded = false,
            Errors = errors,
            ErrorType = errorType
        };
}

public enum AuthErrorType
{
    None,
    Conflict,
    BadRequest,
    Unauthorized
}
