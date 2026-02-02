using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Account;

public class RegisterModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(128, ErrorMessage = "Username cannot exceed 128 characters")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(128, ErrorMessage = "Email cannot exceed 128 characters")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
