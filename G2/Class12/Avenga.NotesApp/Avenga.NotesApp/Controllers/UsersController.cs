using Avenga.NotesApp.Dtos.UserDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avenga.NotesApp.Controllers
{
    [Authorize] // this means that every method will require authorization
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous] // no token needed(we cannot be logged in before registration)
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                _userService.RegisterUser(registerUserDto);
                return StatusCode(StatusCodes.Status201Created, "User Created");
            }
            catch (UserDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")] // no token needed (we cannot be logged in before login)
        public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                string token = _userService.LoginUser(loginUserDto);
                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpGet] // successfull token saving needed in order to access this method
        public IActionResult TestIfLoggedIn()
        {
            return Ok("Successfully accessed after authentication!");
        }
    }
}
