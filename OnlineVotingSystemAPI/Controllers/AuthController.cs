using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Services.Interfaces;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace OnlineVotingSystemAPI.Controllers
{
    // For Login, Logout, and Registration
    //Handles authentication(login, logout, registration)

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //creating Register endpoint
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userDto = await _authService.RegisterUserAsync(registrationDTO);

                //// Serialize the response using Newtonsoft.Json if needed
                //var jsonResponse = JsonConvert.SerializeObject(userDto);

                //return Ok(jsonResponse);

                return Ok(new
                {
                    message = "Successfully registered.",
                    user = userDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //creating Login Endpoint
        // POST: api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            //check if login details are null
            if (loginDTO == null)
            {
                return BadRequest("Login details cannot be null");
            }

            try
            {
                //Call Service to login
                var token = await _authService.LoginUserAsync(loginDTO);

                //if login is successful, show message
                return Ok(new
                {
                    // Success message
                    Message = "Login successful.",

                    // Return the token
                    Token = token
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
