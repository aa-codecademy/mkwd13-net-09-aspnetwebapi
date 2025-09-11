using MoviesApp.Domain.Models;

namespace MoviesApp.DataAccess.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		User GetUserByUsername(string username);
	}
}
