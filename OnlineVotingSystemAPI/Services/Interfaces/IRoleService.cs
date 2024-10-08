using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Services.Interfaces
{
    // Role service interface

    public interface IRoleService
    {
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
