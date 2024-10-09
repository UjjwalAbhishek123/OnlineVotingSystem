using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;

namespace OnlineVotingSystemAPI.Repositories.Impementations
{
    // User repository implementation

    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _dbContext;
        
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Validate user input
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            // Check if user with the same email already exists
            var existingUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            
            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            // Add the new user to the database
            await _dbContext.Users.AddAsync(user);

            //Save changes to database
            await _dbContext.SaveChangesAsync();

            //return created user
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);

            //return true if deleted
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("User not found at this email.");
            }

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Check if the user exists in the database
            var existingUser = await _dbContext.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Update the properties of the existing user
            existingUser.Email = user.Email;

            // Update other properties as needed, for example:
            // existingUser.Password = user.Password;

            // Save changes to the database
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserWithRolesByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception($"User with email '{email}' not found.");
            }

            return user;
        }
    }
}
