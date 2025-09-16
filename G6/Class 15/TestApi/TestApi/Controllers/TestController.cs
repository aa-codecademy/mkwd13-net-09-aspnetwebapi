using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpGet("testUser")]
		public ActionResult<User> GetTestUser()
		{
			User user = new User
			{
				FirstName = "Test",
				LastName = "User",
				UserName = "TestUser"
			};

			return Ok(user);
		}

		[HttpPost]
		public IActionResult AddTestUser([FromBody] User user)
		{
			return Ok();
		}
	}
}
