using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotesApp.DTOs;

namespace NotesApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExternalController : ControllerBase
	{
		//ping TestApi
		//get test user
		//return test user

		//BOTH APIS must be alive, WE MUST RUN BOTH APIS
		[HttpGet]
		public ActionResult<UserDto> GetTestUser()
		{
			try
			{
				using(HttpClient client = new HttpClient())
				{
					HttpResponseMessage message = client.GetAsync("http://localhost:5292/api/Test/testUser").Result;
					string content = message.Content.ReadAsStringAsync().Result; //Json string

					//JSON -> UserDto
					UserDto user = JsonConvert.DeserializeObject<UserDto>(content);
					return Ok(user);
				}

			}catch(Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
