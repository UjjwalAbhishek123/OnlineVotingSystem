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
using OnlineVotingSystemAPI.Helpers;
using System.IO;

namespace OnlineVotingSystemAPI.Repositories.Implementations
{
    //implementing admin functionalities getting data from databsase
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;

        //creating private field for Distributed Cache
        private readonly IDistributedCache _cache;

        public AdminRepository(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var cacheKey = "Get_All_Users";

            //Get data from cache
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                //if data is found in cache, ENCODE and DESERIALIZE cached data
                //Encoding cache data
                var cachedDataString = Encoding.UTF8.GetString(cachedData);

                //deserializing and returning Cache data
                return JsonSerializer.Deserialize<IEnumerable<UserDTO>>(cachedDataString) ?? new List<UserDTO>();
            }

            //If not found in cache, fetch from DATABASE
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
            //Serializing data and Encoding it
            var newDataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userDtos));

            //setting cache options
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            //add Data in Cache
            await _cache.SetAsync(cacheKey, newDataToCache, options);

            return userDtos;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var cacheKey = $"User_{id}";

            //Get data from cache
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                //if data found in cache, Encode
                var cachedDataString = Encoding.UTF8.GetString(cachedData);

                //then, Desrialize cached data
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
            //Serializing data and Encoding data
            var newDataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userDto));

            //setting cache options
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(24))
                .SetSlidingExpiration(TimeSpan.FromHours(12));

            //add data in cache
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

        // Method to get the admin's profile
        public async Task<AdminDTO?> GetAdminProfileAsync(int adminId)
        {
            if (adminId <= 0)
            {
                throw new ArgumentException("Admin ID must be greater than zero.", nameof(adminId));
            }

            var cacheKey = $"Admin_{adminId}";

            //Get data from cache
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                //if data found in cache, Encode
                var cachedDataString = Encoding.UTF8.GetString(cachedData);

                //then, Desrialize cached data
                return JsonSerializer.Deserialize<AdminDTO>(cachedDataString) ?? throw new KeyNotFoundException($"Admin with id {adminId} not found.");
            }

            var admin = await _dbContext.Admins.Include(a => a.Roles)
                .Include(a => a.Roles)
                .ThenInclude(ar => ar.Role)
                .FirstOrDefaultAsync(a => a.Id == adminId);

            if (admin == null)
            {
                return null;
            }

            var adminDTO = new AdminDTO
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                RoleNames = admin.Roles.Select(ar => ar.Role.Name).ToList()
            };

            // Caching the data
            //Serializing data and Encoding data
            var newDataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(adminDTO));

            //setting cache options
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(24))
                .SetSlidingExpiration(TimeSpan.FromHours(12));

            //add data in cache
            await _cache.SetAsync(cacheKey, newDataToCache, options);

            return adminDTO;
        }

        //public async Task<bool> UpdateAdminProfileAsync(Admin admin)
        //{
        //    if (admin == null)
        //    {
        //        throw new ArgumentNullException(nameof(admin), "Admin cannot be null.");
        //    }

        //    //check existing admin
        //    var existingAdmin = await _dbContext.Admins.FindAsync(admin.Id);

        //    if (existingAdmin == null)
        //    {
        //        return false;
        //    }

        //    existingAdmin.FirstName = admin.FirstName;
        //    existingAdmin.LastName = admin.LastName;
        //    existingAdmin.Email = admin.Email;

        //    _dbContext.Admins.Update(existingAdmin);

        //    // Clear cache for users after updating an admin
        //    await _cache.RemoveAsync($"Admin_{admin.Id}");
        //    await _cache.RemoveAsync("Get_All_Users");

        //    return await _dbContext.SaveChangesAsync() > 0;
        //}

        // Method to change the admin password
        //public async Task<bool> ChangeAdminPasswordAsync(int adminId, string oldPassword, string newPassword)
        //{
        //    if (adminId <= 0)
        //    {
        //        throw new ArgumentException("Admin ID must be greater than zero.", nameof(adminId));
        //    }

        //    if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
        //        throw new ArgumentException("Old password and new password cannot be null or empty.");

        //    var admin = await _dbContext.Admins.FindAsync(adminId);

        //    if (admin == null)
        //    {
        //        return false;
        //    }

        //    // Use your password helper to verify the old password
        //    if(!PasswordHelper.VerifyPassword(oldPassword, admin.Password))
        //    {
        //        throw new UnauthorizedAccessException("Old password is incorrect.");
        //    }

        //    // Hash the new password using the password helper
        //    admin.Password = PasswordHelper.HashPassword(newPassword);

        //    return await UpdateAdminProfileAsync(admin);
        //}

        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");

            return await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin?> CreateAdminAsync(Admin admin, List<string> roleNames)
        {
            // Check if the provided admin is null
            if (admin == null)
                throw new ArgumentNullException(nameof(admin), "Admin cannot be null.");

            // Create a new instance of Admin using the data from the provided admin object
            var newAdmin = new Admin
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Password = PasswordHelper.HashPassword(admin.Password) // Hash the password
            };

            // Add the new admin to the database
            await _dbContext.Admins.AddAsync(newAdmin);
            await _dbContext.SaveChangesAsync(); // Save to get the new Admin ID

            // Now create a corresponding User record
            var newUser = new User
            {
                FirstName = newAdmin.FirstName,
                LastName = newAdmin.LastName,
                Email = newAdmin.Email,
                Password = PasswordHelper.HashPassword(admin.Password) // Hash this password as well
                                                                       // Add any additional properties as needed
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync(); // Save to persist the User record

            // Associate roles with the new admin
            foreach (var roleName in roleNames)
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
                if (role != null)
                {
                    var adminRole = new AdminRole
                    {
                        AdminId = newAdmin.Id,
                        RoleId = role.Id
                    };
                    await _dbContext.AdminRoles.AddAsync(adminRole);
                }
            }

            await _cache.RemoveAsync("Get_All_Users");

            await _dbContext.SaveChangesAsync(); // Save changes for AdminRole entries

            return newAdmin; // Return the newly created admin
        }

        public async Task<IEnumerable<CandidateDTO>> GetAllCandidatesAsync(int votingEventID)
        {
            var candidates = await _dbContext.Candidates.ToListAsync();

            var candidateDtos = new List<CandidateDTO>();

            foreach (var candidate in candidates)
            {
                candidateDtos.Add(new CandidateDTO
                {
                    Id = candidate.Id,
                    Name = candidate.Name,
                    Party = candidate.Party,
                    VotingEventId = candidate.VotingEventId
                });
            }

            return candidateDtos;
        }

        public async Task<CandidateDTO> GetCandidateByIdAsync(int candidateId)
        {
            var candidate = await _dbContext.Candidates.FindAsync(candidateId);

            if (candidate == null)
            {
                return null; // Return null if candidate is not found
            }

            return new CandidateDTO
            {
                Id = candidate.Id,
                Name = candidate.Name,
                Party = candidate.Party,
                VotingEventId = candidate.VotingEventId
            };
        }

        public async Task<Candidate> CreateCandidateAsync(Candidate candidate)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate), "Candidate cannot be null.");
            }

            _dbContext.Candidates.Add(candidate);

            await _dbContext.SaveChangesAsync();
            return candidate;
        }

        public async Task<bool> UpdateCandidateAsync(CandidateDTO candidateDto)
        {
            var candidate = await _dbContext.Candidates.FindAsync(candidateDto.Id);
            if (candidate == null)
            {
                return false; // Return false if candidate not found
            }

            // Update candidate properties
            candidate.Name = candidateDto.Name;
            candidate.Party = candidateDto.Party;
            candidate.VotingEventId = candidateDto.VotingEventId;


            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            return true; // Return true if the update was successful
        }

        public async Task<bool> DeleteCandidateAsync(int candidateId)
        {
            var candidate = await _dbContext.Candidates.FindAsync(candidateId);

            if(candidate == null)
            {
                return false;
            }

            _dbContext.Candidates.Remove(candidate);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<VotingEvent> CreateVotingEventAsync(VotingEvent votingEvent)
        {
            if (votingEvent == null)
            {
                throw new ArgumentNullException(nameof(votingEvent), "Voting event cannot be null.");
            }

            await _dbContext.VotingEvents.AddAsync(votingEvent);
            await _dbContext.SaveChangesAsync();
            return votingEvent;
        }

        public async Task<VotingEvent> GetVotingEventByIdAsync(int id)
        {
            var votingEvent = await _dbContext.VotingEvents.FindAsync(id);

            if (votingEvent == null)
            {
                throw new KeyNotFoundException("The specified Voting Event does not exist.");
            }

            return votingEvent;
        }

        public async Task<bool> UpdateVotingEventAsync(VotingEvent votingEvent)
        {
            var existingEvent = await _dbContext.VotingEvents.FindAsync(votingEvent.Id);
            
            if (existingEvent == null) 
                return false;

            existingEvent.EventName = votingEvent.EventName;
            existingEvent.EventDate = votingEvent.EventDate;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVotingEventAsync(int id)
        {
            var votingEvent = await _dbContext.VotingEvents.FindAsync(id);
            
            if (votingEvent == null) 
                return false;

            _dbContext.VotingEvents.Remove(votingEvent);
            
            await _dbContext.SaveChangesAsync();
            
            return true;
        }
    }
}
