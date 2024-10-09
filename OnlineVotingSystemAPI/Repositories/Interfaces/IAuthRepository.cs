using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserResponseDTO> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<Role> GetRoleByNameAsync(string roleName);

        //Adding new Method to get user with roles by Email
        //For login
        Task<User> GetUserWithRolesByEmailAsync(string email);
    }
}
