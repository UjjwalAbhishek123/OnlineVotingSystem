using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;

namespace OnlineVotingSystemAPI.Repositories.Implementations
{
    // Role repository implementation

    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            //Fetch role from DB
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
