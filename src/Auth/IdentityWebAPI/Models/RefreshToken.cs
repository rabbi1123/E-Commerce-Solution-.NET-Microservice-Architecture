namespace IdentityWebAPI.Models;

public class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }

    public bool IsRevoked { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
