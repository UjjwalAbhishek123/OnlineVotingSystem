namespace OnlineVotingSystemAPI.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email {  get; set; }
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}
