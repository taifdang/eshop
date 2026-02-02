using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Account;

public class LoginModel
{
    public string? ReturnUrl { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [StringLength(128, ErrorMessage = "Username cannot exceed 128 characters")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
