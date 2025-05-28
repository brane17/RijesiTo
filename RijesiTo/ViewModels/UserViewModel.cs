using RijesiTo.Models;
using System.ComponentModel.DataAnnotations;

namespace RijesiTo.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserRole Role { get; set; } // "User", "Worker", "Admin"
    }
}
