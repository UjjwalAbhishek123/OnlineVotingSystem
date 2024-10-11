using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineVotingSystemAPI.Repositories.Implementations
{
    //implementing admin functionalities getting data from databsase
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public AdminRepository(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var cacheKey = "Get_All_Users";
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<IEnumerable<UserDTO>>(cachedDataString) ?? new List<UserDTO>();
            }

            var users = await _dbContext.Users
                .Include(u => u.Roles) // Eager load roles
                .ToListAsync();

            var userDtos = users.Select(u => new UserDTO
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Password = u.Password,
                Roles = u.Roles.Select(r => r.Name).ToList()
            }).ToList();

            // Caching the data
            var newDataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userDtos));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            await _cache.SetAsync(cacheKey, newDataToCache, options);

            return userDtos;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var cacheKey = $"User_{id}";
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<UserDTO>(cachedDataString) ?? throw new KeyNotFoundException($"User with id {id} not found.");
            }

            // Finding user by specific id and including roles
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id); //Use FirstOrDefaultAsync for null check

            //fetch a single user based on the provided ID. It returns null if no user is found

            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not Found.");
            }

            var userDto = new UserDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };

            // Caching the data
            var newDataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userDto));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(24))
                .SetSlidingExpiration(TimeSpan.FromHours(12));

            await _cache.SetAsync(cacheKey, newDataToCache, options);

            return userDto;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            //checking if user data is null,
            //then throw an exception
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            //else part
            //add user to database
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Clear user cache on creation
            await _cache.RemoveAsync("Get_All_Users");

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // checking if user data is null,
            //then throw an exception
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            //if user is existing already
            //find the user using his id
            var existingUser = await _dbContext.Users.FindAsync(user.Id);

            //checking if already existing user is null
            //then throw an exception
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            //else make the updates
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            //check if the number of affected rows is greater than zero.
            //If changes were successfully saved, it will return true; otherwise, it returns false
            bool updated = await _dbContext.SaveChangesAsync() > 0;

            // Clear user cache on update
            await _cache.RemoveAsync($"User_{user.Id}");
            await _cache.RemoveAsync("Get_All_Users");

            return updated;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            //finding user at specific id
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                //no deletion occurred because the user does not exist
                return false;
            }

            //if user exists, then remove
            _dbContext.Users.Remove(user);

            //save the changes to the database asynchronously.
            //It returns true if the number of affected rows(deleted users) is greater than zero, indicating a successful deletion.
            //If no rows were affected, it returns false
            bool deleted = await _dbContext.SaveChangesAsync() > 0;

            // Clear user cache on deletion
            await _cache.RemoveAsync($"User_{id}");
            await _cache.RemoveAsync("Get_All_Users");

            return deleted;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            //var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            //This line is giving error in postman as,
            //The LINQ expression 'DbSet<Role>().Where(r => r.Name.Equals(
            //value: __roleName_0,
            //comparisonType: OrdinalIgnoreCase))' could not be translated. Additional information: Translation of the 'string.Equals'
            //overload with a 'StringComparison' parameter is not supported. See https://go.microsoft.com/fwlink/?linkid=2129535 for more
            //information. Either rewrite the query in a form that can be translated,
            //or switch to client evaluation explicitly by inserting a call to 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or 'ToListAsync'.
            //See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.

            //Below line will make case-insensitive comparision, which is compatible for EntityFrameWork.
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());

            if (role == null)
            {
                throw new KeyNotFoundException($"Role '{roleName}' not found.");
            }

            return role;
        }


    }
}
