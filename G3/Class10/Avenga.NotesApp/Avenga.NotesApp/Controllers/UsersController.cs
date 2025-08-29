using Avenga.NotesApp.Dto.Users;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                _userService.RegisterUser(registerUserDto);
                return StatusCode(StatusCodes.Status201Created, "User created!");
            }
            catch (UserDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
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
                return Ok(token);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UserDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
