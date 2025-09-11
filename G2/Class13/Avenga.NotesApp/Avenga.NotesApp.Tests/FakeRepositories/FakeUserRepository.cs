using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;

namespace Avenga.NotesApp.Tests.FakeRepositories
{
    public class FakeUserRepository : IUserRepository
    {
        private List<User> _users;
        public FakeUserRepository()
        {
            _users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "Bob",
                    LastName = "Bobsky",
                    Password = "123456",
                    Username = "Boby_123"
                },
                new User()
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Johnsky",
                    Password = "123456",
                    Username = "Johnny_123"
                }
            };
        }
        public void Add(User entity)
        {
            _users.Add(entity);
        }

        public void Delete(User entity)
        {
            _users.Remove(entity);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User LoginUser(string username, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            _users[_users.IndexOf(entity)] = entity;
        }
    }
}
