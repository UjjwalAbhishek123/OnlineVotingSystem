using Microsoft.Extensions.Caching.Distributed;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Services.Interfaces
{
    public interface IAuthService
    {
        //method to Register user
        Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registrationDTO);

        //Method to Login User
        //type is string because it will generate JWt used for authentication
        Task<string> LoginUserAsync(LoginDTO loginDTO);
    }

}
