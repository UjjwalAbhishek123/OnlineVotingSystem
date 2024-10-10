using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        //Purpose: Retrieves a list of all users.This is essential for an admin to manage users effectively.
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        //Purpose: Retrieves a specific user by their ID. This allows the admin to view details of a particular user.
        Task<UserDTO> GetUserByIdAsync(int id);

        //Purpose: Creates a new user.This is crucial for an admin who can add new users to the system.
        Task<User> CreateUserAsync(User user);

        //Purpose: Updates an existing user’s information.Admins often need to modify user details, such as email or roles.
        Task<bool> UpdateUserAsync(User user);

        //Purpose: Deletes a user by ID.Admins should have the ability to remove users from the system.
        Task<bool> DeleteUserAsync(int id);

        // Only fetching roles
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
