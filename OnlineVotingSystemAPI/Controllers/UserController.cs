using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Controllers
{
    //Manages user-related functionalities(view, edit, delete users

    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;   
        }

        // Sample endpoint to check if user access works
        [HttpGet("get-candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var candidates = await _userService.GetAllCandidateAsync();
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
