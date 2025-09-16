using Avenga.MovieApp.Dtos.UserDto;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.MovieApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                UserDto user = _userService.LoginUser(loginUserDto);
                if(user == null)
                {
                    Log.Error($"Not Found: Someone tried to user the username {loginUserDto.Username} or password to enter our app!");
                    return NotFound("Username or Password is incorrect!");
                }
                Log.Information($"OK: The user with username: {loginUserDto.Username} just logged in the application!");
                return Ok(user);
            }
            catch (Exception ex) 
            {
                Log.Error($"Internal Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto) 
        {
            try
            {
                _userService.RegisterUser(registerUserDto);

                Log.Information($"Successfully registered user with username {registerUserDto.Username}");
                return Ok(new ResponseDto() { Success = "Successfully registered user!"});
            }
            catch (UserException ex)
            {
                Log.Error($"Bad Request user exception: {ex.Message}");
                return BadRequest(new ResponseDto() { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"Internal server error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
