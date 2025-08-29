using Avenga.NotesApp.Domain.Models;

namespace Avenga.NotesApp.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
        User LoginUser(string username, string hashedPassword);
    }
}
