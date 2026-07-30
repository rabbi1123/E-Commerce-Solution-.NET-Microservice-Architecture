using System.ComponentModel.DataAnnotations;

namespace IdentityWebAPI.DTOs;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
