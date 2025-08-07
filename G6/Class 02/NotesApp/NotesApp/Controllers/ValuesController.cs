using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Controllers
{
	[Route("api/[controller]")] //http://localhost:[port]/api/values
	[ApiController]
	public class ValuesController : ControllerBase
	{
		[HttpGet] //no additional info
				  //http://localhost:[port]/api/values
		public List<string> Get()
		{
			return new List<string>() { "value1", "value2" };
		}

		//[HttpGet] //ERROR - has same http method and same address
		//		  //http://localhost:[port]/api/values
		//public int GetNumber()
		//{
		//	return 1;
		//}

		[HttpGet("number")] //http://localhost:[port]/api/values/number
		public int GetNumber()
		{
			return 1;
		}

		[HttpPost] //This is okay, beacuse it uses a different http method, although the route seems the same 
		//http://localhost:[port]/api/values
		public string GetString()
		{
			return "Hello";
		}

		[HttpGet("details/{id}")] //http://localhost:[port]/api/values/details/1
		public int GetId(int id)
		{
			return id;
		}

		[HttpGet("{id}")] //http://localhost:[port]/api/values/1
		public int GetFirstNumber(int id)
		{
			return id;
		}

		[HttpGet("{userId}/book/{bookId}")] //http://localhost:[port]/api/values/1/book/3
		public string GetUserAndBook(int userId, int bookId)
		{
			return $"User: {userId}, book: {bookId}";
		}

		[HttpGet("{number}/movies")] //http://localhost:[port]/api/values/2/movies
		public List<string> GetMovies(int number)
		{
			return new List<string>() { "Deadpool", "Joker" };
		}
	}
}
