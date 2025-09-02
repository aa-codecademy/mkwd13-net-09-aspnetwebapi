using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class UserRepository : IRepository<User>
    {
        private readonly NotesAppDbContext _dbContext;

        public UserRepository(NotesAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User entity)
        {
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _dbContext.Users.Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _dbContext.Users.Include(x => x.Notes).ToList();
        }

        public User GetById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            //if (user == null)
            //{
            //    throw new Exception($"User with id: {id}, not found");
            //}

            return user;
        }

        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
