using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Controllers
{
    // Admin-specific operations (e.g., managing users, votes).

    // Only admins can access this controller
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ApplicationDbContext _dbContext;
        public AdminController(IAdminService adminService, ApplicationDbContext dbContext)
        {
            _adminService = adminService;
            _dbContext = dbContext;
        }

        //GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            //admin get all users
            var users = await _adminService.GetAllUsersAsync();

            //return users with 200 OK response
            return Ok(users);
        }

        //GET: api/admin/users/2
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _adminService.GetUserByIdAsync(id);

                //return user with 200 OK response
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //POST: api/admin/users
        [HttpPost("users")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    return BadRequest("User data is null.");
                }

                // Convert UserDTO to User entity
                var user = new User
                {
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Password = PasswordHelper.HashPassword(userDTO.Password), // Ensure to hash the password before saving
                    Roles = await _dbContext.Roles
                                .Where(r => userDTO.Roles.Contains(r.Name))
                                .ToListAsync() // Fetch roles based on names
                };

                var createdUser = await _adminService.CreateUserAsync(user);

                // Create UserDTO for the response
                var createdUserDto = new UserDTO
                {
                    Email = createdUser.Email,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Password = createdUser.Password,
                    Roles = createdUser.Roles.Select(r => r.Name).ToList()
                };

                //return 201 Created response when user is created
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUserDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT: api/admin/users
        [HttpPut("usersUpdate/{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    return BadRequest("User data is null.");
                }

                // Convert UserDTO to User entity
                var user = new User
                {
                    Id = id, // Set the ID from the route
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    // Optionally handle password
                    Password = PasswordHelper.HashPassword(userDTO.Password), // Hash this password if needed
                    Roles = await _dbContext.Roles
                                .Where(r => userDTO.Roles.Contains(r.Name))
                                .ToListAsync()
                };

                var result = await _adminService.UpdateUserAsync(user);

                if (!result)
                {
                    return NotFound("User not found.");
                }

                // Create UserDTO for the response
                var updatedUserDto = new UserDTO
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Roles = user.Roles.Select(r => r.Name).ToList()
                };

                return Ok(updatedUserDto); // Return UserDTO instead of User
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _adminService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound("User not found.");
            }
            return NoContent();
        }

        // GET: api/admin/roles/{roleName}
        [HttpGet("roles/{roleName}")]
        [Authorize(Roles = "Admin")] // Admin authorization for role fetching
        public async Task<ActionResult<Role>> GetRoleByName(string roleName)
        {
            try
            {
                var role = await _adminService.GetRoleByNameAsync(roleName);
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
