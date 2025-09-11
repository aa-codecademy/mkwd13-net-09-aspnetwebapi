using Avenga.NotesApp.Dtos.NoteDtos;
using Avenga.NotesApp.Services.Implementations;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Tests.FakeRepositories;
using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Shared.CustomExceptions;

namespace Avenga.NotesApp.Tests
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void AddNote_InvalidUserId_Exception()
        {
            //Arrange
            //we make a new instance with our NoteService from the Services Project
            //But we initialize it with the fake repositories, not the real ones
            //because they communicate with the production Database
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());

            //we created new note with invalid user Id
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = "Do your work!",
                UserId = 3
            };

            //We Check if the application will throw an exception if we use our actual service and
            //try to add new note with invalid user id (user that does not exist!)
            //Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));
        }

        [TestMethod]
        public void AddNote_EmptyText_Exception()
        {
            //arrange
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = "",
                UserId = 1
            };

            //Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));
        }

        [TestMethod]
        public void AddNote_LargerText_Exception()
        {
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.Low,
                Tag = Tag.Work,
                Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadfffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj",
                UserId = 1
            };

            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote));
        }

        [TestMethod]
        public void GetAllNotes_Count()
        {
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());
            var expectedCount = 2;
            var result = noteService.GetAllNotes();

            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void GetNoteById_InvalidId_Exception()
        {
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());

            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetById(4));
        }

        [TestMethod]
        public void GetNoteById_ValidUser_NoteDto()
        {
            INoteService noteService = new NoteService(new FakeNotesRepository(), new FakeUserRepository());
            var expectedNoteText = "Do something";

            //var result = noteService.GetById(1);
            NoteDto noteDto = noteService.GetById(1);

            Assert.AreEqual(expectedNoteText, noteDto.Text);
        }
    }
}
