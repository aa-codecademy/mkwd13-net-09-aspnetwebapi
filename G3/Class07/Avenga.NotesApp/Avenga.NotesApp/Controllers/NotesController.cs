using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService) //DI
        {
            _noteService = noteService;
        }

        [HttpGet]
        public ActionResult<List<NoteDto>> GetAllNotes()
        {
            try
            {
                return Ok(_noteService.GetAllNotes());
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
                NoteDto noteDto = _noteService.GetByIdNote(id);
                return Ok(noteDto); //satus code => 200
            }
            catch(NoteNotFoundException e)
            {
                return NotFound(e.Message); // staus code => 404
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); //staus code => 500
            }
        }
    }
}
