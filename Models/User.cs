using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; } = string.Empty;  // Hashed Password

    [Required]
    [Display(Name = "User Type")]
    public string Role { get; set; } = "User";  // Default role
}
