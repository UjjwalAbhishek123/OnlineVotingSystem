using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Services.Implementations
{
    // User service implementation

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        //public async Task<bool> DeleteUserAsync(int id)
        //{
        //    return await _userRepository.DeleteUserAsync(id);
        //}

        public async Task<bool> DeleteUserAsync(int id)
        {
            // Get the user by id
            var user = await _userRepository.GetUserByIdAsync(id);

            // Check if user is null
            if (user == null)
            {
                return false; // User not found
            }

            // If user exists, delete the user
            return await _userRepository.DeleteUserAsync(id);
        }


        //public async Task<User> GetUserByEmailAsync(string email)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(email);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found at this email.");
        //    }

        //    return user;
        //}

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        //public async Task<User> GetUserByIdAsync(int id)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }
        //    return user;
        //}

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }


        public async Task<User> RegisterUserAsync(UserRegisterDTO registrationDTO)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registrationDTO.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var newUser = new User
            {
                Email = registrationDTO.Email,
                Password = PasswordHelper.HashPassword(registrationDTO.Password)
            };

            await _userRepository.CreateUserAsync(newUser);
            return newUser;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            //check if user exists
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Update user properties
            existingUser.Email = user.Email;

            // Update other properties as needed, e.g.:
            //existingUser.Roles = user.Roles;

            return await _userRepository.UpdateUserAsync(existingUser);
        }
    }
}
