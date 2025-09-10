using Avenga.NotesApp.Dto.Users;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                Log.Information($"Registration model info: FirstName : {registerUserDto.FirstName}, LastName: {registerUserDto.LastName}");
                _userService.RegisterUser(registerUserDto);
                Log.Information($"Successfully registered {registerUserDto.Username}");
                return StatusCode(StatusCodes.Status201Created, "User created!");
            }
            catch (UserDataException e)
            {
                Log.Error($"There was na error registering use because of the following error: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginDto loginUserDto)
        {
            try
            {
                string token = _userService.LoginUser(loginUserDto);
                Log.Information($"The user with username: {loginUserDto.Username} just logged in the application");
                return Ok(token);
            }
            catch (UserNotFoundException e)
            {
                Log.Error($"Someone tried to use the username {loginUserDto.Username} to enter our app!");
                return NotFound(e.Message);
            }
            catch (UserDataException e)
            {
                Log.Error($"There was na error registering use because of the following error: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("AuthTest")]
        public IActionResult TestAuth()
        {
            return Ok("Auth successfully implemented and working via swager!");
        }
    }
}
