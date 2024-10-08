using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Services.Interfaces
{
    public interface IAuthService
    {
        //method to Register user
        Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registrationDTO);
    }
}
