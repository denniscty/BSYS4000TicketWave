using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketWave.Models
{
    public enum UserRole
{
    Admin,
    User,
    Moderator
}
    public class User : IdentityUser
    {
        // You can now remove: UserId (already in IdentityUser), PasswordHash, etc.
        
        [Display(Name = "User Type")]
        public string Role { get; set; } = "User";

        // Add custom fields if needed (PhoneNumber already exists in base class)
        // public string SomeOtherField { get; set; }
    }  
}
