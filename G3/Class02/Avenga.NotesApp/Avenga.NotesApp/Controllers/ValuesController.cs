using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet] // http://localhost:[port]/api/values
        public List<string> GetValues()
        {
            return new List<string>() { "values1", "values2" };
        }
       
        //[HttpGet] // http://localhost:[port]/api/values/string
        //public string GetString()
        //{
        //    return "value";
        //}

        [HttpGet("string")] // http://localhost:[port]/api/values/string
        public string GetString()
        {
            return "value";
        }

        [HttpPost]
        public string Post()
        {
            return "string";
        }


        [HttpGet("{id}")] // http://localhost:[port]/api/values/1
        //[HttpGet("id/{id}")]
        public int GetById(int id)
        {
            return id;
        }

        [HttpGet("{id}/book/{bookId}")] // http://localhost:[port]/api/values/1/book/2
        public ActionResult<string> GetByIdAndGetByBookId(int id, int bookId) 
        {
            return $"Value id: {id}, Book id: {bookId}";
        }
    }
}
