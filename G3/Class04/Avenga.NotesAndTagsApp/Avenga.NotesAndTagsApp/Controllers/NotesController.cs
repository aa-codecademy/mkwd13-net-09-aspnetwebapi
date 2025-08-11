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
    }
}
