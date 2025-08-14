using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesAndTagsApp.DTOs;
using NotesAndTagsApp.Models;

namespace NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<NoteDto>> GetAll()
        {
            try
            {
                var notesDb = StaticDb.Notes;
                var notesDto = notesDb.Select(x => new NoteDto
                {
                    Priority = x.Priority,
                    Text = x.Text,
                    User = $"{x.User.FirstName} {x.User.LastName}",
                    Tags = x.Tags.Select(t => t.Name).ToList()
                }).ToList();

                return Ok(notesDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                if(id < 0)
                {
                    return BadRequest("The Id cannot be negative.");
                }

                var noteDb = StaticDb.Notes.SingleOrDefault(x => x.Id == id);
                if(noteDb == null)
                {
                    return NotFound($"Note with id: {id} does not exist!");
                }

                var noteDto = new NoteDto
                {
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }

        [HttpGet("findById")] //http://localhost:[port]/api/notes/findById?id=1
        public ActionResult<NoteDto> FindById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("The Id cannot be negative.");
                }

                var noteDb = StaticDb.Notes.SingleOrDefault(x => x.Id == id);
                if (noteDb == null)
                {
                    return NotFound($"Note with id: {id} does not exist!");
                }

                var noteDto = new NoteDto
                {
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                var noteDb = StaticDb.Notes.FirstOrDefault(
                    x => x.Id == updateNoteDto.Id);
                if (noteDb == null)
                {
                    return NotFound(
                        $"The note with id: {updateNoteDto.Id} was not found");
                }

                if(string.IsNullOrEmpty(updateNoteDto.Text))
                {
                    return BadRequest("Text is a required field");
                }

                var userDb = StaticDb.Users.FirstOrDefault(
                    x => x.Id == updateNoteDto.UserId);
                if (userDb == null)
                {
                    return NotFound(
                        $"User with id: {updateNoteDto.UserId} was not found!");
                }

                var tags = new List<Tag>();
                foreach(int tagId in updateNoteDto.TagIds)
                {
                    var tagDb = StaticDb.Tags.FirstOrDefault(
                        x => x.Id == tagId);

                    if(tagDb == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found");
                    }
                    tags.Add(tagDb);
                }
                
                noteDb.Text = updateNoteDto.Text;
                noteDb.Priority = updateNoteDto.Priority;
                noteDb.User = userDb;
                noteDb.UserId = userDb.Id;
                noteDb.Tags = tags;

                return StatusCode(StatusCodes.Status204NoContent,
                    "Note updated");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }

        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                if (string.IsNullOrEmpty(addNoteDto.Text))
                {
                    return BadRequest("Text is a required field");
                }

                var userDb = StaticDb.Users.FirstOrDefault(
                    x => x.Id == addNoteDto.UserId);
                if (userDb == null)
                {
                    return NotFound(
                        $"User with id: {addNoteDto.UserId} was not found!");
                }

                var tags = new List<Tag>();
                foreach (int tagId in addNoteDto.TagIds)
                {
                    var tagDb = StaticDb.Tags.FirstOrDefault(
                        x => x.Id == tagId);

                    if (tagDb == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found");
                    }
                    tags.Add(tagDb);
                }

                var newNote = new Note
                {
                    Id = StaticDb.Notes.Count + 1,
                    Text = addNoteDto.Text,
                    Priority = addNoteDto.Priority,
                    User = userDb,
                    UserId = addNoteDto.UserId,
                    Tags = tags
                };

                StaticDb.Notes.Add(newNote);
                return StatusCode(StatusCodes.Status201Created,
                    "Note created!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNoteById(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Id has invalid value");
                }

                var noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id);
                if(noteDb == null)
                {
                    return NotFound($"Note with id: {id} was not found!");
                }

                StaticDb.Notes.Remove(noteDb);
                return Ok("Note was deleted!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }
        [HttpGet("user/{userId}")]
        public ActionResult<List<NoteDto>> GetNotesByUser(int userId)
        {
            try
            {
                var userNotes = StaticDb.Notes.Where(x => x.UserId == userId).ToList();
                var userNotesDto = userNotes.Select(x => new NoteDto
                {
                    Priority = x.Priority,
                    Text = x.Text,
                    User = $"{x.User.FirstName} {x.User.LastName}",
                    Tags = x.Tags.Select(t => t.Name).ToList()
                }).ToList();

                return Ok(userNotesDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured, contact admin!");
            }
        }
    }
}
