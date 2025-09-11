using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Domain.Enums;

namespace Avenga.NotesApp.Tests.FakeRepositories
{
    public class FakeNotesRepository : IRepository<Note>
    {
        private List<Note> _notes;
        public FakeNotesRepository()
        {
            _notes = new List<Note>()
             {
                 new Note()
                 {
                     Id = 1,
                     UserId = 1,
                     Priority = Priority.High,
                     Tag = Tag.Health,
                     Text = "Do something",
                     User = new User()
                     {
                         Id = 1,
                         FirstName = "Bob",
                         LastName = "Bobsky",
                         Password = "123456",
                         Username = "Boby_123"
                     }
                     
                 },
                 new Note()
                 {
                     Id = 2,
                     UserId = 2,
                     Priority = Priority.Medium,
                     Tag = Tag.SocialLife,
                     Text = "Do something else!",
                     User = new User()
                     {
                         Id = 2,
                         FirstName = "John",
                         LastName = "Johnsky",
                         Password = "123456",
                         Username = "Johnny_123"
                     }
                 }
             };
        }
        public void Add(Note entity)
        {
            _notes.Add(entity);
        }

        public void Delete(Note entity)
        {
            _notes.Remove(entity);
        }

        public List<Note> GetAll()
        {
            return _notes;
        }

        public Note GetById(int id)
        {
            return _notes.SingleOrDefault(note => note.Id == id);
        }

        public void Update(Note entity)
        {
            _notes[_notes.IndexOf(entity)] = entity;
        }
    }
}
