using Avenga.NotesAndTagsApp.DTOs;
using Avenga.NotesAndTagsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avenga.NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<NoteDto>> GetAllNotes()
        {
            try
            {
                //db
                var notesDb = StaticDb.Notes;
                //map
                var notes = notesDb.Select(x=> new NoteDto
                {
                    Text = x.Text,
                    Priority = x.Priority,
                    User = $"{x.User.FirstName} {x.User.LastName}",
                    Tags = x.Tags.Select(t=>t.Name).ToList()
                }).ToList();

                //response
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetNoteById(int id)
        {
            try
            {
                if(id < 0)
                {
                    return BadRequest("The id can not be negative!");
                }

                Note noteDb = StaticDb.Notes.SingleOrDefault(x=> x.Id == id);
                if (noteDb == null)
                { 
                    return NotFound($"Note with id {id} does not exist!");
                }

                NoteDto noteDto = new NoteDto()
                {
                    Text = noteDb.Text,
                    Priority = noteDb.Priority,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t=>t.Name).ToList()
                };

                return Ok(noteDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("findById")]
        public ActionResult<NoteDto> FindById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("The id can not be negative!");
                }

                Note noteDb = StaticDb.Notes.SingleOrDefault(x => x.Id == id);
                if (noteDb == null)
                {
                    return NotFound($"Note with id {id} does not exist!");
                }

                NoteDto noteDto = new NoteDto()
                {
                    Text = noteDb.Text,
                    Priority = noteDb.Priority,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };

                return Ok(noteDto);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                if (string.IsNullOrEmpty(addNoteDto.Text))
                {
                    return BadRequest("Text os required field!");
                }

                User userDb = StaticDb.Users.SingleOrDefault(x=>x.Id == addNoteDto.UserId);

                if(userDb == null)
                {
                    return NotFound($"User with id {addNoteDto.UserId} was not found");
                }

                List<Tag> tags = new List<Tag>();
                foreach (int tagId in addNoteDto.TagIds) 
                {
                    Tag tagDb = StaticDb.Tags.SingleOrDefault(x => x.Id == tagId);

                    if(tagDb == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found!");
                    }
                    tags.Add(tagDb);
                }

                //CREATE
                Note newNote = new Note()
                {
                    Id = ++StaticDb.NoteId,
                    Text = addNoteDto.Text,
                    Priority = addNoteDto.Priority,
                    UserId = addNoteDto.UserId,
                    User = userDb,
                    Tags = tags
                };
                StaticDb.Notes.Add(newNote);

                return StatusCode(StatusCodes.Status201Created, "Note created!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                Note noteDb = StaticDb.Notes.SingleOrDefault(x=> x.Id == updateNoteDto.Id);
                if(noteDb == null)
                {
                    return NotFound($"Note with id {updateNoteDto.Id} was not found!");
                }
                if (string.IsNullOrEmpty(updateNoteDto.Text))
                {
                    return BadRequest();
                }
                User userDb = StaticDb.Users.SingleOrDefault(x => x.Id == updateNoteDto.UserId);
                if(userDb == null)
                {
                    return NotFound($"User with id {updateNoteDto.UserId} was not found!");
                }
                List<Tag> tags = new List<Tag>();
                foreach (int tagId in updateNoteDto.TagIds) 
                {
                    Tag tagDb = StaticDb.Tags.SingleOrDefault(x=> x.Id == tagId);
                    if(tagDb == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found!");

                    }
                    tags.Add(tagDb);
                }

                //UPDATE
                noteDb.Text = updateNoteDto.Text;
                noteDb.Priority = updateNoteDto.Priority;
                noteDb.UserId = userDb.Id;
                noteDb.User = userDb;
                noteDb.Tags = tags;

                return StatusCode(StatusCodes.Status204NoContent, "Note updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
