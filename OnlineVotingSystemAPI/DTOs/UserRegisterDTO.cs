using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email length can't be more than 100.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name length can't be more than 50.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name length can't be more than 50.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password length can't be more than 255.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; } // Role as a required field for assigning role during registration
    }
}
