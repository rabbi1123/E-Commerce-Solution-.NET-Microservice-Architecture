using Microsoft.AspNetCore.Identity;

namespace IdentityWebAPI.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
