using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Services.Implementations;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using NotesAppTests.FakeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppTests
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void AddNote_InvalidUserId_Exception()
        {
            //Arrange
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            int userId = 3;
            var newNote = new AddNoteDto()
            {
                Priority = Priority.High,
                Tag = Tag.Health,
                Text = "Drink water"
            };

            //Assert
            Assert.ThrowsException<NoteDataException>(()=> noteService.AddNote(newNote, userId));
        }

        [TestMethod]
        public void AddNote_EmptyText_Exception()
        {
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.High,
                Tag = Tag.Health,
                Text = ""
            };

            //Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote, 1));
        }

        [TestMethod]
        public void AddNote_LargerText_Exception()
        {
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            var newNote = new AddNoteDto()
            {
                Priority = Priority.High,
                Tag = Tag.Health,
                Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            };

            //Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(newNote, 3));
        }




        [TestMethod]
        public void GetAllNotes_Count_AreEqual()
        {
            //Arrange
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            int expectedCount = 2;
            int userId = 1;
            //Act
            var result = noteService.GetAllNotes(userId);

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void GetByIdNote_ValidUser_NoteDto()
        {
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            string expectedNoteText = "Do something";
            int noteId = 1;

            var result = noteService.GetByIdNote(noteId);

            Assert.AreEqual(expectedNoteText, result.Text);
        }

        [TestMethod]
        public void GetByIdNote_InvalidId_Exception()
        {
            INoteService noteService = new NoteService(new FakeNoteRepository(), new FakeUserRepository());
            int noteId = 3;
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetByIdNote(noteId));
        }

    }
}
