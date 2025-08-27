using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        public NotesAppDbContext _noteAppDbContext;
        public UserRepository(NotesAppDbContext noteAppDbContext)
        {
            _noteAppDbContext = noteAppDbContext; //DI
        }
        public void Add(User entity)
        {
            _noteAppDbContext.Users.Add(entity);
            _noteAppDbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _noteAppDbContext.Users.Remove(entity);
            _noteAppDbContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return _noteAppDbContext.Users.SingleOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _noteAppDbContext.Users.SingleOrDefault(x=> x.Username.ToLower() == username.ToLower());
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _noteAppDbContext.Users.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == hashedPassword);
        }

        public void Update(User entity)
        {
            _noteAppDbContext.Users.Update(entity);
            _noteAppDbContext.SaveChanges();
        }
    }
}
