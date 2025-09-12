using Avenga.NotesApp.Dtos.UserDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
                Log.Information($"Registration model info: FirstName: {registerUserDto.FirstName}, LastName: {registerUserDto.LastName}");
                _userService.RegisterUser(registerUserDto);
                Log.Information($"Successfully registered {registerUserDto.Username}");
                return StatusCode(StatusCodes.Status201Created, "User Created");
            }
            catch (UserDataException e)
            {
                Log.Error($"User data exception, check input values: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
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
                Log.Information($"The user with username {loginUserDto.UserName} just logged in the app!");
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                Log.Information($"User with id {id} was retrieved from the database");
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                Log.Error($"User was not found: {ex.Message}");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                Log.Information($"All Users were retrieved from the db, Number of users currently {users.Count()}");
                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpPut]
        public IActionResult UpdateUser(UpdateUserDto updateUserDto)
        {
            try
            {
                _userService.UpdateUser(updateUserDto);
                Log.Information($"User with id {updateUserDto.Id} was updated!");
                return Ok();
            }
            catch (UserDataException ex)
            {
                Log.Error($"User data exception, check input values: {ex.Message}");
                return BadRequest();
            }
            catch (UserNotFoundException ex)
            {
                Log.Error($"User was not found: {ex.Message}");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, contact the Admin!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                Log.Information($"User with id {id} was deleted from the database!");
                //return NoContent(); // Like this we cannot give a message
                return StatusCode(StatusCodes.Status204NoContent, "User was successfully deleted!");
            }
            catch (UserNotFoundException ex)
            {
                Log.Error($"User was not found: {ex.Message}");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
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
