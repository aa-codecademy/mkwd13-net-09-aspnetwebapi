using Avenga.NotesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.DataAccess.Implementations
{
    public class UserRepository : IRepository<User>
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

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
