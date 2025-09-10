using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppTests.FakeRepository
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
                    LastName = "Bobski",
                    Username = "bob",
                    Password = "bob123",
                    Age = 19
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
            return _users.SingleOrDefault(x => x.Id == id);
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
            int index = _users.FindIndex(x=> x.Id == entity.Id);
            if(index != -1)
            {
                _users[index] = entity;
            }
        }
    }
}
