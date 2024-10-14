using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Services.Interfaces;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                return Ok(new RoleDTO { Id = role.Id, Name = role.Name });
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

        // GET: api/admin/profile/{id}
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetAdminProfile(int id)
        {
            try
            {
                var profile = await _adminService.GetAdminProfileAsync(id);

                return Ok(profile);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT: api/admin/updateAdmin-profile/{id}
        //[HttpPut("updateAdmin-profile/{id}")]
        //public async Task<IActionResult> UpdateAdminProfile(int id, [FromBody] Admin admin)
        //{
        //    if (admin == null || admin.Id != id)
        //    {
        //        return BadRequest("Admin data cannot be null and must match the provided ID.");
        //    }

        //    try
        //    {
        //        var result = await _adminService.UpdateAdminProfileAsync(admin);

        //        if (result)
        //        {
        //            return NoContent(); // 204 No Content
        //        }
        //        return NotFound("Admin not found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        // POST: api/admin/change-password/{adminId}
        //[HttpPost("change-password/{adminId}")]
        //public async Task<IActionResult> ChangeAdminPassword(int adminId, [FromBody] ChangePasswordDTO changePasswordDTO)
        //{
        //    try
        //    {
        //        var result = await _adminService.ChangeAdminPasswordAsync(adminId, changePasswordDTO);

        //        if (result)
        //        {
        //            return NoContent(); // 204 No Content
        //        }
        //        return BadRequest("Password change failed. Please check your old password.");
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminDTO adminDto)
        {
            // Check if the request body is null
            if (adminDto == null)
            {
                return BadRequest("User information cannot be null.");
            }

            // Validate user data
            if (string.IsNullOrWhiteSpace(adminDto.FirstName) ||
                string.IsNullOrWhiteSpace(adminDto.LastName) ||
                string.IsNullOrWhiteSpace(adminDto.Email) ||
                string.IsNullOrWhiteSpace(adminDto.Password))
            {
                return BadRequest("First name, last name, email, and password are required.");
            }

            // Check for valid email format
            if (!IsValidEmail(adminDto.Email))
            {
                return BadRequest("Invalid email format.");
            }

            // Check if the admin already exists
            var existingAdmin = await _adminService.GetAdminByEmailAsync(adminDto.Email);
            if (existingAdmin != null)
            {
                return Conflict("An admin with this email already exists.");
            }

            // Map DTO to Admin model
            var admin = new Admin
            {
                FirstName = adminDto.FirstName,
                LastName = adminDto.LastName,
                Email = adminDto.Email,
                Password = PasswordHelper.HashPassword(adminDto.Password) // Hash the password
            };

            // Create the admin and user in the database
            var createdAdmin = await _adminService.CreateAdminAsync(admin, adminDto.RoleNames);

            // Optional: You can retrieve the user here if you want to return user data as well
            // var createdUser = await _userService.GetUserByEmailAsync(createdAdmin.Email);

            var createdAdminDto = new AdminDTO
            {
                Id = createdAdmin!.Id,
                FirstName = createdAdmin.FirstName,
                LastName = createdAdmin.LastName,
                Email = createdAdmin.Email,
                RoleNames = adminDto.RoleNames
            };

            // Return created response
            return CreatedAtAction(nameof(GetAdminProfile), new { id = createdAdmin.Id }, createdAdminDto);
        }

        // Email validation helper method
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Voting Event Endpoints
        [HttpGet("voting-events/{id}")]
        public async Task<IActionResult> GetVotingEventById(int id)
        {
            var votingEvent = await _adminService.GetVotingEventByIdAsync(id);

            if (votingEvent == null)
            {
                return NotFound("The specified Voting Event does not exist.");
            }

            return Ok(new VotingEventDTO
            {
                Id = votingEvent.Id,
                EventName = votingEvent.EventName,
                EventDate = votingEvent.EventDate
            });
        }


        [HttpPost("voting-events")]
        public async Task<IActionResult> CreateVotingEvent([FromBody] VotingEventDTO votingEventDto)
        {
            if (votingEventDto == null)
                return BadRequest("Voting event data is required.");

            var votingEvent = new VotingEvent
            {
                EventName = votingEventDto.EventName,
                EventDate = votingEventDto.EventDate,
            };

            var createdEvent = await _adminService.CreateVotingEventAsync(votingEvent);

            // Return the created voting event as a DTO
            return CreatedAtAction(nameof(GetVotingEventById), new { id = createdEvent.Id }, new VotingEventDTO
            {
                Id = createdEvent.Id,
                EventName = createdEvent.EventName,
                EventDate = createdEvent.EventDate
            });
        }

        [HttpPut("voting-events/{id}")]
        public async Task<IActionResult> UpdateVotingEvent(int id, [FromBody] VotingEvent votingEvent)
        {
            if (id != votingEvent.Id) 
                return BadRequest("Voting event ID mismatch.");
            
            var result = await _adminService.UpdateVotingEventAsync(votingEvent);
            
            if (!result) 
                return NotFound($"Voting event with ID {id} not found.");
            
            return NoContent();
        }

        [HttpDelete("voting-events/{id}")]
        public async Task<IActionResult> DeleteVotingEvent(int id)
        {
            var result = await _adminService.DeleteVotingEventAsync(id);
            
            if (!result) 
                return NotFound($"Voting event with ID {id} not found.");
            
            return NoContent();
        }

        //endpint for getting all candidates based on voting event Id
        [HttpGet("voting-events/{votingEventId}/candidates")]
        public async Task<IActionResult> GetAllCandidates(int votingEventId)
        {
            var candidates = await _adminService.GetAllCandidatesAsync(votingEventId);

            return Ok(candidates);
        }

        [HttpGet("candidates-by-id/{id}")]
        public async Task<IActionResult> GetCandidateById(int id)
        {
            var candidate = await _adminService.GetCandidateByIdAsync(id);

            if (candidate == null)
            {
                return NotFound($"Candidate with ID {id} not found."); // Return 404 if candidate not found
            }

            return Ok(candidate); // Return candidate data
        }

        [HttpPost("create-candidates")]
        public async Task<IActionResult> CreateCandidates([FromBody] CandidateDTO candidateDto)
        {
            // Check if the request body is null
            if (candidateDto == null)
            {
                return BadRequest("Candidate information cannot be null.");
            }

            // Check if the VotingEventId exists
            var votingEventExists = await _dbContext.VotingEvents.AnyAsync(e => e.Id == candidateDto.VotingEventId);
            if (!votingEventExists)
            {
                return BadRequest("The specified Voting Event does not exist.");
            }

            var createdCandidate = await _adminService.CreateCandidateAsync(candidateDto);

            return CreatedAtAction(nameof(GetCandidateById), new { id = createdCandidate.Id }, createdCandidate);
        }

        [HttpPut("update-candidates/{id}")]
        public async Task<IActionResult> UpdateCandidates(int id, [FromBody] CandidateDTO candidateDto)
        {
            if (candidateDto == null)
            {
                return BadRequest("Candidate data is required.");
            }

            if (id != candidateDto.Id)
            {
                return BadRequest("Candidate ID mismatch.");
            }

            var result = await _adminService.UpdateCandidateAsync(candidateDto);

            if (result)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound($"Candidate with ID {id} not found.");
            }
        }

        [HttpDelete("delete-candidates/{candidateId}")]
        public async Task<IActionResult> DeleteCandidate(int candidateId)
        {
            var result = await _adminService.DeleteCandidateAsync(candidateId);

            if (!result)
            {
                return NotFound($"Candidate with ID {candidateId} not found.");
            }

            return NoContent();
        }
    }
}
