using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Repositories.Interfaces
{
    // Role repository interface

    public interface IRoleRepository
    {
        //Method to get Role by name
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
