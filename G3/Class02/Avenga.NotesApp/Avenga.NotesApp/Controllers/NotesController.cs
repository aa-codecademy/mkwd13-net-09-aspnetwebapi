using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<string>> GetAll()
        {
            //return Ok(StaticDb.SimpleNotes);
            return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes);
        }

        [HttpGet("{index}")]
        public IActionResult GetByIndex(int index)
        {
            try
            {
                if (index < 0) 
                {
                    return BadRequest("The index must be positive number");
                    //return StatusCode(StatusCodes.Status400BadRequest, "The index must be positive number");
                }

                if (index > StaticDb.SimpleNotes.Count)
                {
                    //return NotFound($"There is no resource on index {index}");

                    return StatusCode(StatusCodes.Status404NotFound, $"There is no resource on index {index}");
                }
                return Ok(StaticDb.SimpleNotes[index]);

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred. Contact the admin!");
            }
        }

        [HttpPost]
        public IActionResult CreateNote()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                { 
                    string newNotes = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(newNotes))
                    {
                        return BadRequest();
                    }

                    StaticDb.SimpleNotes.Add(newNotes);
                    return StatusCode(StatusCodes.Status201Created, "The new note was added");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred. Contact the admin!");
            }
        }
    }
}
