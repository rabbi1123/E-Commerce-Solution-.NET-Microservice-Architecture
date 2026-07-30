using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
