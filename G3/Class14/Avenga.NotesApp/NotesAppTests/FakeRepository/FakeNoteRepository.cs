using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppTests.FakeRepository
{
    public class FakeNoteRepository : IRepository<Note>
    {
        private List<Note> _notes;
        public FakeNoteRepository() 
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
                    User = new User{Id = 1, Username = "bob"}
                },
                new Note()
                {
                    Id = 2,
                    UserId = 1,
                    Priority = Priority.Medium,
                    Tag = Tag.Work,
                    Text = "Do something else!",
                    User = new User{Id = 1, Username = "bob"}
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
            return _notes.SingleOrDefault(x=> x.Id == id);
        }

        public void Update(Note entity)
        {
            _notes[_notes.IndexOf(entity)] = entity;
        }
    }
}
