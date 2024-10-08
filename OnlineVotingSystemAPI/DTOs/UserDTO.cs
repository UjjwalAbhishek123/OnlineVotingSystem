using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.DTOs
{
    //DTO for user data transfer
    public class UserDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email length can't be more than 100.")]
        public string Email { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password length can't be more than 255.")]
        public string Password { get; set; }
    }
}
