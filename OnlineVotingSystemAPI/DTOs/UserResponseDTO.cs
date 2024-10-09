namespace OnlineVotingSystemAPI.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email {  get; set; }
        public string FirstName { get; set; } // Add this
        public string LastName { get; set; }  // Add this
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}
