using Moq;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Services.Implementation;
using NotesApp.Services.Interfaces;

namespace NotesApp.Tests
{
	[TestClass]
	public class NoteServiceUnitTests
	{
		//we want to focus only on testing the impl of the note service
		//that is why we will mock the repos that we would need in order to test the impl of the methods in the note service
		private readonly INoteService _noteService;
		private readonly Mock<IRepository<Note>> _noteRepository;
		private readonly Mock<IRepository<User>> _userRepository;

		public NoteServiceUnitTests()
		{
			_noteRepository = new Mock<IRepository<Note>>();
			_userRepository = new Mock<IRepository<User>>();
			_noteService = new NoteService(_noteRepository.Object, _userRepository.Object);
		}

		[TestMethod]
		public void GetAllNotes_should_return_NotesDtos()
		{
			//Arange - the test data that we will need in order to test the GetAllNotes method
			FilterDto filterDto = new FilterDto();
			int userId = 1;

			//we need the "fake" data that would be returned from the repo call
			List<Note> notes = new List<Note>()
			{
				new Note
				{
					Id = 1,
					Tag = Domain.Enums.TagEnum.AvengaAcademy,
					Text = "Do your homework",
					Priority = Domain.Enums.PriorityEnum.High,
					UserId = 1,
					User = new User
					{
						Id = 1,
						FirstName = "Petko",
						LastName = "Petkovski"
					}
				},
				new Note
				{
					Id = 2,
					Tag = Domain.Enums.TagEnum.Exercise,
					Text = "Go to the gym",
					Priority = Domain.Enums.PriorityEnum.Medium,
					UserId = 2,
					User = new User
					{
						Id = 2,
						FirstName = "Marko",
						LastName = "Markovski"
					}
				}
			};

			//instead of calling GetAll from the repo, return the notes from the arrange part of the test
			_noteRepository.Setup(x => x.GetAll()).Returns(notes);

			//Act 
			List<NoteDto> noteDtos = _noteService.GetAllNotes(filterDto, userId);

			//Assert
			Assert.AreEqual(1, noteDtos.Count);
			Assert.AreEqual("Do your homework", noteDtos.First().Text);
			Assert.AreEqual("Petko Petkovski", noteDtos.First().UserFullName);
		}

		[TestMethod]
		public void GetAllNotes_ShouldReturnEmptyList_on_EmptyListFromDb()
		{
			//Arrange
			FilterDto filterDto = new FilterDto();
			int userId = 1;

			//simulate that no data was returned from the db (from repo)
			_noteRepository.Setup(x => x.GetAll()).Returns(new List<Note>());

			//Act
			List<NoteDto> noteDtos = _noteService.GetAllNotes(filterDto, userId);

			//Assert
			Assert.AreEqual(0, noteDtos.Count);
		}

		[TestMethod]
		public void GetById_ShoudlThrow_On_NoteNotFound()
		{
			//Arrange
			int id = 2;
			int userId = 1;
			//simulate that the note with id 2 was not found - the db returned null
			_noteRepository.Setup(x => x.GetById(id)).Returns(null as Note);

			//Act and Assert
			var exception = Assert.ThrowsException<NullReferenceException>(() => _noteService.GetById(id, userId));
		}
	}
}
