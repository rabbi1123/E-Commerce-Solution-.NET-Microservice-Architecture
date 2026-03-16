using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace User.API.Models;

public class ApplicationUser : IdentityUser<int>
{
    [Required]
    public bool IsActive { get; set; } = true;

    [StringLength(100)]
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? UserType { get; set; }

    public virtual List<RoleUser> RoleUsers { get; set; } = new();
}

