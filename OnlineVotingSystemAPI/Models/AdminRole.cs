using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.Models
{
    public class AdminRole
    {
        [Required(ErrorMessage = "AdminId is required.")]
        public int AdminId { get; set; }
        public Admin Admin { get; set; }

        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
