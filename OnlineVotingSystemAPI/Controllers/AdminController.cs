using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystemAPI.Controllers
{
    // Admin-specific operations (e.g., managing users, votes).

    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        //sample access to check if admin access works
        [HttpGet("test")]
        public IActionResult TestAdminEndPoint()
        {
            return Ok("Admin endpoint is working!");
        }

        // Sample endpoint for getting all users
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            return Ok(new { message = "List of users (not implemented yet)." });
        }

        // Sample endpoint for deleting a user
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            return Ok(new { message = $"User with ID {id} deleted (not implemented yet)." });
        }
    }
}
