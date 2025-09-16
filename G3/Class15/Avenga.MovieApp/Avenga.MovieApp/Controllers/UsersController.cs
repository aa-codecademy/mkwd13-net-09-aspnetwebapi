using Avenga.MovieApp.Dtos.UserDto;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    return NotFound("Username or Password is incorrect!");
                }
                return Ok(user);
            }
            catch(UserException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex) 
            {
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

                return Ok(new ResponseDto() { Success = "Successfully registered user!"});
            }
            catch (UserException e)
            {
                return BadRequest(new ResponseDto() { Error = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
