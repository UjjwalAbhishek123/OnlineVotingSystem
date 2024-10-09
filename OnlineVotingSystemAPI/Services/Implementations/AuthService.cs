using Microsoft.IdentityModel.Tokens;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace OnlineVotingSystemAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        //private readonly field for AuthRepository
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        //RegisterUser method implementation
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

        //LoginUser method implementation
        public async Task<string> LoginUserAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                throw new ArgumentNullException(nameof(loginDTO), "Login details cannot be null.");
            }

            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
            {
                throw new ArgumentException("Email and Password cannot be empty.");
            }

            //Get user with role by email
            var user = await _authRepository.GetUserWithRolesByEmailAsync(loginDTO.Email);

            //check if user is null and VerifyPassword doesn't match with the password in loginDTO and password entered by User
            //Throw an exception of invalid Login attempt
            if(user == null|| !PasswordHelper.VerifyPassword(loginDTO.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }

            //Generate JWT Token
            var token = GenerateJwtToken(user);

            return token;
        }

        //Method for JWT token generation
        private string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            //Defining claims
            var claims = new List<Claim>
            {
                //adding roles
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
            };

            // Adding roles to claims
            //If we dont use loop then only first role will be added as claim, if we have multiples.
            //So use foreach loop
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            //Creating SymmetricSecurityKey
            //implementing Null check also, coz, key can be null
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? 
                throw new InvalidOperationException("JWT Key is not configured.")));

            //creating signIn credentials
            var signInCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //creating JwtSecurityToken
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],        
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signInCreds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
