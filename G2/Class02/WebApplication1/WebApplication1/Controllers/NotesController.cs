using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")] //http://localhost:[port]/api/notes
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<string>> Get()
        {
            //return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes);
            return BadRequest(StaticDb.SimpleNotes);
        }

        //Wont work because it has the same address and method [GET] as the above method
        //[HttpGet] //http://localhost:[port]/api/notes
        //public ActionResult<List<string>> Get()
        //{
        //    return new List<string>
        //    {
        //        "123",
        //        "456"
        //    };
        //}

        [HttpGet("{index}")] ////http://localhost:[port]/api/notes/1
        public ActionResult<string> GetByIndex(int index)
        {
            try
            {
                if (index < 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        "The index has negative value");
                }

                if (index >= StaticDb.SimpleNotes.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        $"There is no resourse on index {index}");
                }

                return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes[index]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred. Contact The admin");
            }
        }

        [HttpPost] //http://localhost:[port]/api/notes/
        public IActionResult Post([FromBody] string newNote)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    //string newNote = reader.ReadToEnd();

                    //if (string.IsNullOrEmpty(newNote))
                    //{
                    //    return BadRequest("The body of the request cannot be empty!");
                    //}

                    StaticDb.SimpleNotes.Add(newNote);
                    return StatusCode(StatusCodes.Status201Created,
                        "The new note was added");
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred. Contact The admin");
            }
        }

    }
}
