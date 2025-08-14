using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Models.DTOs;
using NotesApp.Models.Enums;

namespace NotesApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController] //http://localhost:[port]/api/notes
	public class NotesController : ControllerBase
	{
		[HttpGet] //http://localhost:[port]/api/notes
		public ActionResult<List<NoteDto>> GetAll()
		{
			try
			{
				//return Ok(StaticDb.Notes);

				var notesDb = StaticDb.Notes;
				//the select linq iterates the notesDb list and for each note selects a new NoteDto where it maps the properties
				var notesDto = notesDb.Select(x => new NoteDto
				{
					Text = x.Text,
					Priority = x.Priority,
					User = $"{x.User.FirstName} {x.User.LastName}",
					Tags = x.Tags.Select(t => $"{t.Name} - {t.Color}").ToList()
				}).ToList();

				return Ok(notesDto);

			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		//[HttpGet("{index}")] //http://localhost:[port]/api/notes/1
		////Here we have a path param that is a part of the route
		//public ActionResult<Note> GetNoteByIndex(int index)
		//{
		//	try
		//	{
		//		if(index < 0)
		//		{
		//			return BadRequest("The index cannot be negative");
		//		}
		//		if(index >= StaticDb.Notes.Count)
		//		{
		//			return NotFound($"There is no resource on index {index}");
		//		}
		//		return Ok(StaticDb.Notes[index]);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
		//	}
		//}


		[HttpGet("{id}")] //http://localhost:[port]/api/notes/1
							 //Here we have a path param that is a part of the route
		public ActionResult<NoteDto> GetNoteById(int id)
		{
			try
			{
				if (id < 0)
				{
					return BadRequest("The index cannot be negative");
				}
				Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id); //get from db

				if(noteDb == null)
				{
					return NotFound($"The note with id {id} does not exist");
				}
				var noteDto = new NoteDto
				{
					Text = noteDb.Text,
					Priority = noteDb.Priority,
					User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
					Tags = noteDb.Tags.Select(x => $"{x.Name} - {x.Color}").ToList()
				};
				return Ok(noteDto);
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

		[HttpGet("language")]
		//in postman we should send key value pair in Headers tab where the key would be the name of our param
		public IActionResult GetAppLanguageFromHeader([FromHeader] string language)
		{
			return Ok(language);
		}

		[HttpGet("userAgent")]
		//in postman we should send key value pair in Headers tab where the key would be the Name we specified for our param (User-Agent)
		public IActionResult GetUserAgentFromHEader([FromHeader(Name = "User-Agent")] string userAgent)
		{
			return Ok(userAgent);
		}

		[HttpPut("updateNote/{index}")] //the index param will be sent in the path while the tag will be sent in the body
		public IActionResult AddNoteToTag(int index, [FromBody] Tag tag){
			try
			{
				//validations
				if (index < 0)
				{
					return BadRequest("The index cannot be negative");
				}
				if(index >= StaticDb.Notes.Count)
				{
					return NotFound($"There is no note on index {index}");
				}
				var noteDb = StaticDb.Notes[index];
				if(noteDb.Tags == null) //in case the Tags list is null, we need to create an empty tag list, otherwise null.Add will throw an exception
				{
					noteDb.Tags = new List<Tag>();
				}
				noteDb.Tags.Add(tag);
				return StatusCode(StatusCodes.Status204NoContent, "Note updated");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("addNote")]
		public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
		{
			try
			{
				//validation
				if(addNoteDto == null)
				{
					return BadRequest("Note cannot be null");
				}
				if (string.IsNullOrEmpty(addNoteDto.Text))
				{
					return BadRequest("Each note must containt text");
				}

				User userDb = StaticDb.Users.FirstOrDefault(x => x.Id == addNoteDto.UserId);
				if(userDb == null)
				{
					return NotFound($"User with id {addNoteDto.UserId} does not exist");
				}

				List<Tag> tags = new List<Tag>();  //1,2 
				foreach(var tagId in addNoteDto.TagIds)
				{
					Tag tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);
					if(tagDb == null)
					{
						return NotFound($"Tag with id {tagId} does not exist");
					}
					tags.Add(tagDb);
				}

				//create 
				Note newNote = new Note
				{
					Id = StaticDb.Notes.LastOrDefault() != null ? StaticDb.Notes.LastOrDefault().Id + 1 : 1,
					Text = addNoteDto.Text,
					Priority = addNoteDto.Priority,
					User = userDb,
					UserId = userDb.Id, //addNoteDto.UserId
					Tags = tags
				};

				//write to db
				StaticDb.Notes.Add(newNote);
				return StatusCode(StatusCodes.Status201Created, "Note created");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("user/{userId}")]
		public ActionResult<List<NoteDto>> GetNoteByUser(int userId)
		{
			try
			{
				if (userId == null)
				{
					return BadRequest("UserId cannot be null");
				}

				var userNotes = StaticDb.Notes.Where(x => x.UserId == userId).ToList();

				var userNotesDtos = userNotes.Select(x => new NoteDto
				{
					Text = x.Text,
					Priority = x.Priority,
					User = $"{x.User.FirstName} {x.User.LastName}",
					Tags = x.Tags.Select(t => $"{t.Name} - {t.Color}").ToList()
				}).ToList();

				return Ok(userNotesDtos);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public IActionResult Update([FromBody] UpdateNoteDto updateNoteDto)
		{
			try
			{
				//validations
				if (updateNoteDto == null)
				{
					return BadRequest("Note cannot be null");
				}

				//we need to find the note that we want to update in the db (also check if it exists)
				Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == updateNoteDto.Id);
				if(noteDb == null)
				{
					return NotFound($"Note with id {updateNoteDto.Id} was not found");
				}
				User userDb = StaticDb.Users.FirstOrDefault(x => x.Id == updateNoteDto.UserId);
				if(userDb == null) //ex. userId = 45
				{
					return NotFound($"User with id {updateNoteDto.UserId} does not exist");
				}
				if (string.IsNullOrEmpty(updateNoteDto.Text))
				{
					return BadRequest("Each note must contain text");
				}
				if((int)updateNoteDto.Priority < 1 || (int)updateNoteDto.Priority > 3)
				{
					return BadRequest("Priority is not valid");
				}

				List<Tag> tags = new List<Tag>();  //1,2 
				foreach (var tagId in updateNoteDto.TagIds)
				{
					Tag tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);
					if (tagDb == null)
					{
						return NotFound($"Tag with id {tagId} does not exist");
					}
					tags.Add(tagDb);
				}

				//update
				noteDb.Text = updateNoteDto.Text;
				noteDb.Priority = updateNoteDto.Priority;
				noteDb.User = userDb;
				noteDb.UserId = userDb.Id; //updateNoteDto.UserId
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
