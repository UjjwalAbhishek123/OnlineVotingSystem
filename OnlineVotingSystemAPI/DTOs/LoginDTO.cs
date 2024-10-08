using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.DTOs
{
    //DTO for login requests
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email length can't be more tha 100.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } //For Login
    }
}
