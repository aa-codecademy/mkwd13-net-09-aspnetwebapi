using Avenga.MovieApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviesDbContext _db;

        public UserRepository(MoviesDbContext db)
        {
            _db = db;
        }

        public void Add(User entity)
        {
            _db.Users.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(User entity)
        {
            _db.Users.Remove(entity);
            _db.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _db.Users.Include(x=>x.MovieList).ToList();
        }

        public User GetById(int id)
        {
            return _db.Users.SingleOrDefault(x => x.Id == id);
        }

        public void Update(User entity)
        {
            _db.Users.Update(entity);
            _db.SaveChanges();
        }
    }
}
