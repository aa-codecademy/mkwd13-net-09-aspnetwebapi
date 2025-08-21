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
			throw new NotImplementedException();
		}

		public void Delete(Note entity)
		{
			throw new NotImplementedException();
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

		public Note GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(Note entity)
		{
			throw new NotImplementedException();
		}
	}
}
