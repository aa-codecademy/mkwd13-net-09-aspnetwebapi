using Avenga.NotesAndTagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avenga.NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<Note>> GetAll()
        {
            try
            {
                return Ok(StaticDb.Notes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpGet("{index}")] //http://localhost:[port]/api/notes/1
        public ActionResult<Note> GetByIndex(int index)
        {
            try
            {
                if (index < 0)
                {
                    return BadRequest("The index can not be negative!");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"There is no resource on index {index}");
                }

                return Ok(StaticDb.Notes[index]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpGet("queryString")]
        public ActionResult<Note> GetByIndexWithQueryString(int? index)
        {
            try
            {
                if (index == null)
                {
                    return BadRequest("Index is a required parameter");
                }
                if (index < 0)
                {
                    return BadRequest();
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"There is no resource on index {index}");
                }
                return Ok(StaticDb.Notes[index.Value]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpGet("text/{text}/priority/{priority}")]
        //[HttpGet("{text}/{priority}")]
        public ActionResult<List<Note>> FilterNote (string text, int priority)
        {
            try
            {
                if(string.IsNullOrEmpty(text) || priority < 1)
                {
                    return BadRequest("Filter parameters are required");
                }
                if(priority > 3)
                {
                    return BadRequest("Invalid value for priority, enter 1, 2 or 3");
                }

                List<Note> notesDb = StaticDb.Notes.Where(x=>x.Text.ToLower().Contains(text.ToLower()) && (int)x.Priority == priority).ToList();

                return Ok(notesDb);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpGet("multipleFilter")]
        public ActionResult<List<Note>> FilterNotesByMultipleParam(string? text, int? priority) 
        {
            try
            {
                if (string.IsNullOrEmpty(text) && priority == null)
                {
                    return BadRequest();
                }
                if (string.IsNullOrEmpty(text)) 
                {
                    List<Note> filterNotesByPriority = StaticDb.Notes.Where(x => (int)x.Priority == priority).ToList();

                    return Ok(filterNotesByPriority);
                }
                if(priority == null)
                {
                    List<Note> filterNotesByText = StaticDb.Notes.Where(x=> x.Text.ToLower().Contains(text.ToLower())).ToList();
                    return Ok(filterNotesByText);
                }

                List<Note> filterNote = StaticDb.Notes.Where(x => x.Text.ToLower().Contains(text.ToLower()) && (int)x.Priority == priority).ToList();

                return Ok(filterNote);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpGet("userAgent")]
        public IActionResult GetHeader([FromHeader(Name = "User-Agent")] string userAgent) 
        {
            return Ok(userAgent);
        }

        [HttpGet("testHeader")]
        public IActionResult GetTestHeader([FromHeader(Name = "TestHeader")] string testHeader) 
        {
            return Ok(testHeader);
        }

        [HttpPost]
        public IActionResult CreateNote([FromBody] Note note)
        {
            try
            {
                if (string.IsNullOrEmpty(note.Text))
                {
                    return BadRequest("Note text must not be empty");
                }
                if(note.Tags == null || note.Tags.Count == 0)
                {
                    return BadRequest("Note must contain tags");
                }

                StaticDb.Notes.Add(note);
                return StatusCode(StatusCodes.Status201Created, "Note created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }

        [HttpPut("updateNote/{index}")]
        public IActionResult UpdateNote(int index, [FromBody] Tag tag)
        {
            try
            {
                if(index < 0)
                {
                    return BadRequest("The index can not be negative!");
                }
                if(index >= StaticDb.Notes.Count)
                {
                    return BadRequest();
                }

                Note noteDb = StaticDb.Notes[index];
                if (noteDb == null)
                {
                    noteDb.Tags = new List<Tag>();
                }
                noteDb.Tags.Add(tag);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred! Contact the admin!");
            }
        }
    }
}
