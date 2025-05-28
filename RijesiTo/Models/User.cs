using System.ComponentModel.DataAnnotations;

namespace RijesiTo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public UserRole Role { get; set; }

    }

    public enum UserRole
    {
        Worker,
        Client,
        Admin
    }
}
