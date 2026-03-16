using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.API.Models;

public class RoleUser : BaseEntity
{
    [Required(ErrorMessage = "Foreign key user id is required.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Foreign key role id is required.")]
    public int RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? ApplicationUser { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual ApplicationRole? ApplicationRole { get; set; }
}

