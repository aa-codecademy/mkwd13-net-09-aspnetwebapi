using Avenga.NotesApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Avenga.NotesApp.DataAccess.Implementations
{
    public class NoteRepository : IRepository<Note>
    {
        private NotesAppDbContext _notesAppDbContext;
        public NoteRepository(NotesAppDbContext notesAppDbContext) //DI
        {
            _notesAppDbContext = notesAppDbContext;
        }
        public void Add(Note entity)
        {
            _notesAppDbContext.Notes.Add(entity);
            _notesAppDbContext.SaveChanges(); //call to db
        }

        public void Delete(Note entity)
        {
            _notesAppDbContext.Notes.Remove(entity);
            _notesAppDbContext.SaveChanges(); //call to db
        }

        public List<Note> GetAll()
        {
            return _notesAppDbContext.Notes
                .Include(x=>x.User) //join Notes table with Users table //.ThenInclude(x=>x.SomeTableInUserClass)
                .ToList();
        }

        public Note GetById(int id)
        {
            return _notesAppDbContext.Notes.Include(x => x.User).SingleOrDefault(x => x.Id == id);
        }

        public void Update(Note entity)
        {
            _notesAppDbContext.Notes.Update(entity);
            _notesAppDbContext.SaveChanges(); //call to db
        }
    }
}
