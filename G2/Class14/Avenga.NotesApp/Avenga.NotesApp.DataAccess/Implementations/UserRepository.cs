using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;

namespace Avenga.NotesApp.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly NotesAppDbContext _notesAppDbContext;

        public UserRepository(NotesAppDbContext notesAppDbContext) 
        {
            _notesAppDbContext = notesAppDbContext;
        }

        public void Add(User entity)
        {
            _notesAppDbContext.Users.Add(entity);
            _notesAppDbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _notesAppDbContext.Users.Remove(entity);
            _notesAppDbContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _notesAppDbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _notesAppDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _notesAppDbContext.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _notesAppDbContext.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()
            && x.Password == hashedPassword); 
        }

        public void Update(User entity)
        {
            _notesAppDbContext.Users.Update(entity);
            _notesAppDbContext.SaveChanges();
        }
    }
}
