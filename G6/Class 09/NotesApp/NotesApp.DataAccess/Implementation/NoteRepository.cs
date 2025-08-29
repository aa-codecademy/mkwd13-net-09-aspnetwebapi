using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class NoteRepository : IRepository<Note>
    {
        private NotesAppDbContext _dbContext; //we need the dbContext in order to access the db

        public NoteRepository(NotesAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Note entity)
        {
            _dbContext.Notes.Add(entity);
            //_dbContext.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(Note entity)
        {
            _dbContext.Notes.Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<Note> GetAll()
        {
            //Select *
            //From notes N
            //join Users U on N.UserId = U.Id - we need this join in order to access the data for the users

            return _dbContext //we use the dbContext
                .Notes //to access our db set
                .Include(x => x.User) //join Notes table with the users table in order to be able to access info about the users
                .ToList(); //and return a list of all notes
        }

        //public List<Note> GetAll(string searchText)
        //{
        //    //Select *
        //    //From notes N
        //    //join Users U on N.UserId = U.Id - we need this join in order to access the data for the users

        //    //var result = _dbContext.Database.SqlQueryRaw<Note>(@"SELECT *
        //    //													FROM Notes n
        //    //													INNER JOIN Users u on n.UserId = u.Id
        //    //													WHERE n.Text LIKE @SearchText", new { @SearchText = $"%{searchText}%" }).ToList();

        //    //var result = _dbContext.Notes.FromSqlRaw(@"SELECT *
        //    //         										FROM Notes n
        //    //         										INNER JOIN Users u on n.UserId = u.Id
        //    //         										WHERE n.Text LIKE @SearchText", new { @SearchText = $"%{searchText}%" }).ToList();

        //    //return result;
        //}

        public Note GetById(int id)
        {
            var note = _dbContext.Notes.Include(x => x.User).FirstOrDefault(x => x.Id == id);

            if (note == null)
            {
                throw new ArgumentNullException($"Note with id: {id}, not found");
            }

            return note;
        }

        public void Update(Note entity)
        {
            _dbContext.Notes.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
