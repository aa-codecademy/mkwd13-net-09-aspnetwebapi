using Avenga.NotesApp.Dtos.NoteDtos;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public ActionResult<List<NoteDto>> GetAll()
        {
            try
            {
                var notes = _noteService.GetAllNotes();
                Log.Information("All notes were retrieved!");
                return Ok(notes);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                var noteDto = _noteService.GetById(id); //potential NoteNotFounException
                Log.Information($"Retrieved note: {noteDto.Text}");
                return Ok(noteDto); // status code => 200
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning($"The requested note was not found! {ex.Message}");
                return NotFound(ex.Message); // status code => 404
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                Log.Information("New note was added!");
                return StatusCode(StatusCodes.Status201Created, "New Note Added");
            }
            catch (NoteDataException ex)
            {
                Log.Warning("Data Error while adding new note");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!");
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                return NoContent(); // 204
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message); // 404
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message); //400
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!"); // 500
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                return Ok($"Note with id {id} successfully deleted!");
            }
            catch (NoteNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the Admin!"); // 500
            }
        }
    }
}
