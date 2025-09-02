using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Helpers;
using NotesApp.Services.Interfaces;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NotesApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                throw new DataException("Model cannot be null");
            }

            ValidationHelper.ValidateRequiredStringColumnLength(registerUserDto.FirstName, "FirstName", 50);
            ValidationHelper.ValidateColumnLength(registerUserDto.LastName, "LastName", 50);
            ValidationHelper.ValidateRequiredStringColumnLength(registerUserDto.Username, "Username", 30);

            if (string.IsNullOrEmpty(registerUserDto.Password) || string.IsNullOrEmpty(registerUserDto.ConfirmPassword))
            {
                throw new DataException("Password is required!");
            }

            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new DataException("Password must match!");
            }

            string strongPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$";

            if (!Regex.IsMatch(registerUserDto.Password, strongPasswordRegex))
            {
                throw new DataException("Password is not strong password!");
            }

            //if(_userRepository.GetAll().Any(x => string.Equals(x.Username, registerUserDto.Username, StringComparison.InvariantCultureIgnoreCase)))
            //{
            //    throw new DataException("User with that username already exists");
            //}

            if (_userRepository.GetAll().Any(x => x.Username == registerUserDto.Username))
            {
                throw new DataException("User with that username already exists");
            }

            //Username: Risto
            //Password: risto123 => hash: asdasd2133 (??+??)
            User user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Username = registerUserDto.Username,
                Password = GenerateHash(registerUserDto.Password),
            };

            _userRepository.Add(user);
        }
        public string Login(LoginUserDto loginUserDto)
        {
            if (loginUserDto == null)
            {
                throw new DataException("Model cannot be null");
            }

            if(string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new DataException("Username and password are required!");
            }

            var hashedPassword = GenerateHash(loginUserDto.Password);

            var userDb = _userRepository.GetAll().FirstOrDefault(x => x.Username == loginUserDto.Username && x.Password == hashedPassword);

            if(userDb == null)
            {
                throw new DataException("Wrong username or password");
            }

            return "User is logged in";
        }

        private string GenerateHash(string password)
        {
            using (var md5Hash = MD5.Create())
            {
                var passwordBytes = Encoding.ASCII.GetBytes(password);
                var hashedBytes = md5Hash.ComputeHash(passwordBytes);
                var hashed = Encoding.ASCII.GetString(hashedBytes);

                return hashed;
            }
        }
    }
}
