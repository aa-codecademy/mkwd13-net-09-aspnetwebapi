using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Services.Interfaces;
using System.Security.Claims;

namespace NotesApp.Controllers
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
        [Authorize]
        public ActionResult<List<NoteDto>> GetAll([FromQuery] FilterDto filter) {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity == null)
                {
                    throw new ArgumentNullException("Identity is null");
                }

                if(!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                {
                    throw new Exception("Claim id does not exists");
                }

                var result = _noteService.GetAllNotes(filter, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddNote([FromBody] AddNoteDto note)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity == null)
                {
                    throw new ArgumentNullException("Identity is null");
                }

                if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                {
                    throw new Exception("Claim id does not exists");
                }

                _noteService.AddNote(note, userId);
                return StatusCode(StatusCodes.Status201Created, "Note created");
            }
            catch(ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity == null)
                {
                    throw new ArgumentNullException("Identity is null");
                }

                if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                {
                    throw new Exception("Claim id does not exists");
                }

                var note = _noteService.GetById(id, userId);
                return Ok(note);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteById(int id) {
            try
            {
                _noteService.DeleteById(id);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

		[HttpPut]
		[Authorize]
		public IActionResult UpdateNote([FromBody] UpdateNoteDto note)
		{
			try
			{
				var identity = HttpContext.User.Identity as ClaimsIdentity;

				if (identity == null)
				{
					throw new ArgumentNullException("Identity is null");
				}

				if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
				{
					throw new Exception("Claim id does not exists");
				}

                note.UserId = userId;

				_noteService.UpdateNote(note);
				return NoContent(); //204
			}
			catch (ArgumentNullException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
