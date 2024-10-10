using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineVotingSystemAPI.Controllers
{
    //Manages user-related functionalities(view, edit, delete users

    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Sample endpoint to check if user access works
        [HttpGet("test")]
        public IActionResult TestUserEndpoint()
        {
            return Ok("User endpoint is working!");
        }

        // Sample endpoint for getting user details
        [HttpGet("me")]
        public IActionResult GetMyDetails()
        {
            return Ok(new { message = "User details (not implemented yet)." });
        }

        // Sample endpoint for updating user details
        [HttpPut("me")]
        public IActionResult UpdateMyDetails()
        {
            return Ok(new { message = "User details updated (not implemented yet)." });
        }
    }
}
