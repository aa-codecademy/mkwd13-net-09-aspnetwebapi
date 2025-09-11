using Microsoft.EntityFrameworkCore;
using MoviesApp.DataAccess.Interfaces;
using MoviesApp.Domain.Models;

namespace MoviesApp.DataAccess.Implementations
{
	public class UserRepository : IUserRepository
	{
		private readonly MoviesDbContext _dbContext;
		public UserRepository(MoviesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void Add(User entity)
		{
			_dbContext.Users.Add(entity);
			_dbContext.SaveChanges(); //we need to call save changes in order to make changes in the db
		}

		public void Delete(User entity)
		{
			_dbContext.Users.Remove(entity);
			_dbContext.SaveChanges(); //in the methods where we change something and we want it to reflect in the db, we need to use saveChanges (add, update, delete)
		}

		public List<User> GetAll()
		{
			return _dbContext
				.Users
				.Include(x => x.MoviesList)
				//.ThenInclude(x => x.Cinema)
				.ToList();
		}

		public User GetById(int id)
		{
			return _dbContext.Users
				.Include(x => x.MoviesList)
				.FirstOrDefault(x => x.Id == id);
		}

		public User GetUserByUsername(string username) 
		{
			return _dbContext.Users
				.Include(x => x.MoviesList)
				.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
		}

		public void Update(User entity)
		{
			_dbContext.Users.Update(entity);
			_dbContext.SaveChanges();
		}
	}
}
