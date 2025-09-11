using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

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
                int userId = GetAuthrizedUserId();
                return Ok(_noteService.GetAllNotes(userId));
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
                Log.Information($"Retrieved note: {noteDto.Text}", DateTime.UtcNow);
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
                int userId = GetAuthrizedUserId();
                _noteService.AddNote(addNoteDto, userId);
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
                int userId = GetAuthrizedUserId();
                _noteService.UpdateNote(updateNoteDto, userId);
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

        private int GetAuthrizedUserId()
        {
            if(!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                throw new UserDataException("NameIdentifier calims not exist!");
            }
            return userId;
        }
    }
}
