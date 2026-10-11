namespace IdentityWebAPI.Services;

public interface IEmailVerificationTokenService
{
    Task<string> IssueAsync(Guid userId);

    Task<AuthOperationResult> VerifyAsync(string? rawToken);
}
