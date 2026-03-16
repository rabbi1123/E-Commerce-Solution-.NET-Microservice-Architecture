using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace User.API.Models;

public class ApplicationRole : IdentityRole<int>
{
    [Required]
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual List<RoleUser> RoleUsers { get; set; } = new();
}

