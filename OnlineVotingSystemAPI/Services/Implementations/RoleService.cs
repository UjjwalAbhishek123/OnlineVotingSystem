using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Services.Implementations
{
    // Role service implementation

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleRepository.GetRoleByNameAsync(roleName);

            if (role == null)
            {
                throw new Exception("Role not found");
            }
            return role;
        }
    }
}
