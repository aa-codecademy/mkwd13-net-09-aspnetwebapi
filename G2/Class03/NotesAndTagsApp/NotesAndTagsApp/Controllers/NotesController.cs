using Microsoft.AspNetCore.Mvc;
using NotesAndTagsApp.Models;

namespace NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<Note>> Get()
        {
            try
            {
                return Ok(StaticDb.Notes);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpGet("{index}")] //http://localhost:[port]/api/notes/1
        public ActionResult<Note> GetNoteByIndex(int index)
        {
            try
            {
                if(index <0)
                {
                    return BadRequest("The index cannot be negative");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"There is no resourse on index {index}");
                }

                return Ok(StaticDb.Notes[index]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpGet("queryString")] //http:localhost:[port]/api/notes/queryString?index=1
        public ActionResult<Note> GetByQueryString(int? index) //int? means it is optional parameter
        {
            try
            {
                if(index == null)
                {
                    return BadRequest("Index is a required parameter");
                }
                if (index < 0)
                {
                    return BadRequest("The index cannot be negative");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"There is no resourse on index {index}");
                }

                return Ok(StaticDb.Notes[index.Value]);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpGet("{text}/priority/{priority}")] //http://localhost:[port]/api/notes/[gym]/priority/1 (it will return all notes whose text contains gym and priority is low)
        public ActionResult<List<Note>> FilterNotes(string text, int priority)
        {
            try
            {
                if(string.IsNullOrEmpty(text) || priority <= 0)
                {
                    return BadRequest("Filter parameters are required!");
                }
                if(priority > 3)
                {
                    return BadRequest("Invalid value for priority");
                }

                List<Note> notesDb = StaticDb.Notes.Where(x => x.Text.ToLower().Contains(text.ToLower()) && (int)x.Priority == priority).ToList();
                return Ok(notesDb);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpGet("multipleQueryParams")]
        public ActionResult<List<Note>> FilterNotesByMultipleParams(string? text, int? priority)
        {
            try
            {
                if (string.IsNullOrEmpty(text) && priority == null)
                {
                    return BadRequest("You have to send at least one filter parameter");
                }
                if (string.IsNullOrEmpty(text))
                {
                    //priority has value
                    List<Note> filteredNotes = StaticDb.Notes.Where(x => (int)x.Priority == priority).ToList();
                    return Ok(filteredNotes);
                }
                if(priority == null)
                {
                    //text has value
                    List<Note> filteredNotes = StaticDb.Notes.Where(x => x.Text.ToLower().Contains(text.ToLower())).ToList();
                    return Ok(filteredNotes);
                }

                //text and priority have values

                List<Note> notesDb = StaticDb.Notes.Where(x => x.Text.ToLower().Contains(text.ToLower()) && (int)x.Priority == priority).ToList();
                return Ok(notesDb);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpGet("header")]
        public IActionResult GetHeader([FromHeader(Name = "TestHeader")] string testHeader)
        {
            return Ok(testHeader);
        }

        [HttpGet("userAgent")]
        public IActionResult GetUserAgentHeader([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok(userAgent);
        }

        [HttpPost]
        public IActionResult PostNote([FromBody] Note note)
        {
            try
            {
                if (string.IsNullOrEmpty(note.Text)) 
                {
                    return BadRequest("Note text must not be empty");
                }
                if(note.Tags == null || note.Tags.Count == 0)
                {
                    return BadRequest("Note must contain Tags!");
                }
                StaticDb.Notes.Add(note);
                return StatusCode(StatusCodes.Status201Created, "New Note Created"); 
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }

        [HttpPut("updateNote/{index}")]
        public IActionResult UpdateNote(int index, [FromBody] Tag tag)
        {
            try
            {
                if (index < 0)
                {
                    return BadRequest("The index cannot be negative");
                }
                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"There is no resourse on index {index}");
                }

                Note noteDb = StaticDb.Notes[index];

                if(noteDb.Tags == null)
                {
                    noteDb.Tags = new List<Tag>();
                }
                noteDb.Tags.Add(tag);
                return StatusCode(StatusCodes.Status204NoContent, "Note update!");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured! Contact the admin {e.Message}");
            }
        }
    }
}
