using System.ComponentModel.DataAnnotations;

namespace User.API.Models;

public class User : BaseEntity
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }
    
    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(500)]
    public string? Photo { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }
}
