using System.ComponentModel.DataAnnotations;

namespace ShoesForFeet.Models
{
    public class User
    {
        public int Id { get; set; } // Unique identifier for the user

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string Name { get; set; } // User name

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        public string Password { get; set; } // Password
    }
}