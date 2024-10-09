using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.Models
{
    // User model class

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email length can't be more tha 100.")]
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

        //Navigational Property for Roles for user
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();


    }
}
