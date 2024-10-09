using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using System.Diagnostics;

namespace OnlineVotingSystemAPI.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        //Creating private readonly field for DbContext to handle DB operations
        private readonly ApplicationDbContext _dbContext;

        //Injecting Dependency
        public AuthRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty.");
            }

            var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            return role;
        }

        public async Task<UserResponseDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            // Map User to UserResponseDTO
            return new UserResponseDTO
            {
                Email = user.Email,
                Roles = user.Roles?.Select(r => new RoleDTO
                {
                    Name = r.Name
                }).ToList() ?? new List<RoleDTO>() // Initialize to an empty list if user.Roles is null
            };
        }

        public async Task<User> GetUserWithRolesByEmailAsync(string email)
        {
            var user =  await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception($"User with email '{email}' not found.");
            }

            return user;
        }
    }
}
