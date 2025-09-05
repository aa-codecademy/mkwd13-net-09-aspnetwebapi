using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Authorize]
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
                Log.Information($"Retrieved note: {noteDto.Text}");
                return Ok(noteDto); //satus code => 200
            }
            catch(NoteNotFoundException e)
            {
                Log.Warning($"The requested note was not found! {e.Message}");
                return NotFound(e.Message); // staus code => 404
            }
            catch (Exception ex) 
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); //staus code => 500
            }
        }

        [HttpPost]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto) 
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                Log.Information($"New note was added!");
                return StatusCode(StatusCodes.Status201Created, "Note added!"); //staus code => 201
            }
            catch(NoteDataException e)
            {
                return BadRequest(e.Message); //staus code => 400
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); //staus code => 500
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                return NoContent(); // staus code => 204
            }
            catch (NoteNotFoundException e)
            {
                return NotFound(e.Message);//staus code => 404
            }
            catch(NoteDataException e)
            {
                return BadRequest(e.Message); //staus code => 400
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); //staus code => 500
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
            catch(NoteNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); //staus code => 500
            }
        }
    }
}
