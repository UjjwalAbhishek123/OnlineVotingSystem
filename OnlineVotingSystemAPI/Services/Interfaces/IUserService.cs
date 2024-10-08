using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Services.Interfaces
{
    // User service interface

    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserRegisterDTO registrationDTO);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
