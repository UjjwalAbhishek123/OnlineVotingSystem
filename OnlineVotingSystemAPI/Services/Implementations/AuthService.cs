using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        //private readonly field for AuthRepository
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registrationDTO)
        {
            if (string.IsNullOrEmpty(registrationDTO.Email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(registrationDTO.Email));
            }

            //checking existing user based on email
            var existingUser = await _authRepository.GetUserByEmailAsync(registrationDTO.Email);

            //if user already exists, throw user already exists exception
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            //comparing password while registering
            if (registrationDTO.Password != registrationDTO.ConfirmPassword)
            {
                throw new Exception("Password do not match");
            }

            if (string.IsNullOrEmpty(registrationDTO.Password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(registrationDTO.Password));
            }

            //if user is null, then register new User
            var newUser = new User()
            {
                //new user will be created based on Email as username
                Email = registrationDTO.Email,

                //Hashing pasword
                Password = PasswordHelper.HashPassword(registrationDTO.Password),

                FirstName = registrationDTO.FirstName,

                LastName = registrationDTO.LastName
            };

            if (string.IsNullOrEmpty(registrationDTO.Role))
            {
                throw new ArgumentException("Role cannot be null or empty.", nameof(registrationDTO.Role));
            }

            //Assign role based on User Input
            var role = await _authRepository.GetRoleByNameAsync(registrationDTO.Role);

            if (role != null)
            {
                newUser.Roles.Add(role);
            }
            else
            {
                throw new Exception("Role not found.");
            }

            //return await _authRepository.CreateUserAsync(newUser);

            // Save the new user
            var createdUser = await _authRepository.CreateUserAsync(newUser);

            // Return a simplified UserResponseDTO
            return new UserResponseDTO
            {
                // Add properties you want to expose in the response
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName, // Add FirstName to response
                LastName = createdUser.LastName,   // Add LastName to response
                Roles = createdUser.Roles.Select(r => new RoleDTO { Name = r.Name }).ToList() //map the roles
            };
        }
    }
}
