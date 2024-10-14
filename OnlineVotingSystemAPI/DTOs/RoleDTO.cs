using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.DTOs
{
    //DTO for role data transfer
    public class RoleDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Role name length can't be more than 50.")]
        public string Name { get; set; }
    }
}
