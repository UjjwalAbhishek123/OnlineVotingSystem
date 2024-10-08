using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Repositories.Interfaces
{
    // User repository interface

    public interface IUserRepository
    {
        //Methods related to users
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
