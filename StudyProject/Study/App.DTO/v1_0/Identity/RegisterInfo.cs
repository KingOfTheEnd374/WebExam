using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0.Identity;

public class RegisterInfo
{
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Email { get; set; } = default!;
    
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Password { get; set; } = default!;
    
    [StringLength(16, MinimumLength = 3, ErrorMessage = "Incorrect length")]
    public string Username { get; set; } = default!;
}