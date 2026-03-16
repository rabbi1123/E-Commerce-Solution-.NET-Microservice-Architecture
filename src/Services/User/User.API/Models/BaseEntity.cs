using System.ComponentModel.DataAnnotations;

namespace User.API.Models;

public abstract class BaseEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;
}


