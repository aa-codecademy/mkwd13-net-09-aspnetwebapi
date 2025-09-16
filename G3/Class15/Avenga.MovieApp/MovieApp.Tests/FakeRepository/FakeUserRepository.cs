using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Models;

namespace MovieApp.Tests.FakeRepository
{
    public class FakeUserRepository : IUserRepository
    {
        private List<User> _users;
        public FakeUserRepository()
        {
            _users = new List<User>()
            {
                new User {Id = 1, FirstName="Alice", LastName="Alice", Username="alice", Password="alice123", MovieList = new List<Movie>()},
                new User {Id = 2, FirstName="Bob", LastName="Bob", Username="bob", Password="Bob123", MovieList = new List<Movie>()},
            };
        }
        public void Add(User entity)
        {
            _users.Add(entity);
        }

        public void Delete(User entity)
        {

            var user = _users.SingleOrDefault(x => x.Id == entity.Id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(x => x.Id == id);
        }

        public void Update(User entity)
        {
            var user = _users.SingleOrDefault(x => x.Id == entity.Id);
            if (user != null)
            {
                var index = _users.IndexOf(user);
                _users[index] = entity;
            }
        }
    }
}
