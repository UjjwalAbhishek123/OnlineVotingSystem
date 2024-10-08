using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.Models
{
    //Role model class
    //Defines roles for users(Admin, User, etc.)

    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Role name length can't be more than 50.")]
        public string Name { get; set; }

        //Navigational property for user having roles
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
