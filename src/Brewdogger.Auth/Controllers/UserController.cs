using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Brewdogger.Auth.Entities;
using Brewdogger.Auth.Exceptions;
using Brewdogger.Auth.Helpers;
using Brewdogger.Auth.Models;
using Brewdogger.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Brewdogger.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Error("UserController::CreatUser - Error in user model stata [{0}]", ModelState.ErrorCount);
                return BadRequest("User is not valid for creation");
            }

            User newUser;

            try
            {
                newUser = _userService.CreateUser(userDto);
            }
            catch (Exception e)
            {
                return ProcessBadCreateUserRequest(e, userDto);
            }
            
            return Created($"User {newUser.Username} created", new { newUser.Username, newUser.Email });
        }
        
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Error("UserController::Login - Error in login model state [{0}]", ModelState.ErrorCount);
                return BadRequest("Login is not valid");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Username or password invalid");

            User user;
            string tokenString;

            try
            {
                user = _userService.Authenticate(loginDto.Username, loginDto.Password);
                tokenString = _userService.CreateToken(user);
            }
            catch (Exception e)
            {
                return ProcessBadLoginRequest(e, loginDto);
            }

            if (user == null || string.IsNullOrWhiteSpace(tokenString))
            {
                Log.Error("UserController::Login - Error retrieving user [{0}]", loginDto.Username);
                return BadRequest("Error retrieving user credentials");
            }
            
            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Email,
                Token = tokenString
            });
        }

        private IActionResult ProcessBadLoginRequest(Exception exception, LoginDto loginDto)
        {
            Log.Error("UserController::ProcessBadLoginRequest - Bad request for user [{0}]", loginDto.Username);
            Log.Error("UserController::ProcessBadLoginRequest - [{0}]", exception.Message);

            if (exception.GetType().IsAssignableFrom(typeof(UserNotFoundException)))
                return NotFound(exception.Message);
            
            return BadRequest(exception.Message);
        }

        private IActionResult ProcessBadCreateUserRequest(Exception exception, UserDto userDto)
        {
            Log.Error("UserController::ProcessBadCreateUserRequest - Bad user creation request for user [{0}]", userDto.Username);
            Log.Error("UserController::ProcessBadCreateUserRequest - [{0}]", exception.Message);

            return BadRequest(exception.Message);
        }
    }
}