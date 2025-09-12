using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Dtos;
using MoviesApp.Services.Interfaces;
using System.Data;

namespace MoviesApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
		{
			try
			{
				_userService.RegisterUser(registerUserDto);
				return StatusCode(StatusCodes.Status201Created, "User created");
			}
			catch (DataException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public ActionResult<string> Login([FromBody] LoginUserDto user)
		{
			try
			{
				var token = _userService.Login(user);
				return Ok(token);
			}
			catch (DataException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
