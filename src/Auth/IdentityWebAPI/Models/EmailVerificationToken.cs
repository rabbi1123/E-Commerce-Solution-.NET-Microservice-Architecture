namespace IdentityWebAPI.Models;

public class EmailVerificationToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }

    public DateTime? UsedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
