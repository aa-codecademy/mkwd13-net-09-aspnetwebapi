using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Models.Enums;

namespace NotesApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController] //http://localhost:[port]/api/notes
	public class NotesController : ControllerBase
	{
		[HttpGet] //http://localhost:[port]/api/notes
		public ActionResult<List<Note>> GetAll()
		{
			try
			{
				return Ok(StaticDb.Notes);

			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{index}")] //http://localhost:[port]/api/notes/1
		//Here we have a path param that is a part of the route
		public ActionResult<Note> GetNoteByIndex(int index)
		{
			try
			{
				if(index < 0)
				{
					return BadRequest("The index cannot be negative");
				}
				if(index >= StaticDb.Notes.Count)
				{
					return NotFound($"There is no resource on index {index}");
				}
				return Ok(StaticDb.Notes[index]);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("multipleQuery")] //http://localhost:[port]/api/notes/multipleQuery
								   //http://localhost:[port]/api/notes/multipleQuery?text=gym
								   //http://localhost:[port]/api/notes/multipleQuery?priority=1
								   //http://localhost:[port]/api/notes/multipleQuery?text=gym&priority=2
								   //http://localhost:[port]/api/notes/multipleQuery?priority=2&text=gym
		public ActionResult<List<Note>> FilterNotesWithQueryParams(string? text, int? priority)
		{
			try
			{
				//validations
				if(string.IsNullOrEmpty(text) && priority == null)
				{
					return BadRequest("You need to send at least one filter param");
				}
				if (string.IsNullOrEmpty(text)) //&& priority != null - at least one of the filters had to be sent
				{
					if(priority <= 0 || priority > 3)
					{
						return BadRequest("Invalid value for priority");
					}
					List<Note> filteredNotesByPriority = StaticDb.Notes
						.Where(x => (int)x.Priority == priority) //we need to cast, so that we can compare the values (cannot compare different types)
						.ToList();
					return Ok(filteredNotesByPriority);
				}
				if(priority == null) // !string.IsNullOrEmpty(text) - at least one of the filters had to be sent
				{
					List<Note> filteredNotesByText = StaticDb.Notes
						.Where(x => x.Text.ToLower().Contains(text.ToLower()))
						.ToList();

					return filteredNotesByText;
				}

				List<Note> filteredNotes = StaticDb.Notes
					.Where(x => x.Text.ToLower().Contains(text.ToLower())
					&& x.Priority == (PriorityEnum)priority)
					.ToList();

				return Ok(filteredNotes);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		[HttpPost]
		public IActionResult CreatePostNote([FromBody] Note note)
		{
			try
			{
				//validation
				if(note == null)
				{
					return BadRequest("Note cannot be null");
				}
				if (string.IsNullOrEmpty(note.Text))
				{
					return BadRequest("Each note must contain text");
				}
				if((int)note.Priority < 1 || (int)note.Priority > 3)
				{
					return BadRequest("Inavalid value for priority");
				}
				if(note.Tags == null || note.Tags.Count == 0)
				{
					return BadRequest("All notes must have some tags");
				}
				if(StaticDb.Notes.Any(x => x.Text == note.Text))
				{
					return BadRequest("Note already exists");
				}
				StaticDb.Notes.Add(note);
				return StatusCode(StatusCodes.Status201Created, "Note has been created");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
