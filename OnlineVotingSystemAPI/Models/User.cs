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
        [StringLength(255, ErrorMessage = "Password length can't be more than 255.")]
        public string Password { get; set; }

        //Navigational Property for Roles for user
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();


    }
}
