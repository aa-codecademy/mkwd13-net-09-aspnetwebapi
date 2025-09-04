using Microsoft.IdentityModel.Tokens;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Helpers;
using NotesApp.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Claims;
using NotesApp.Domain.Enums;

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

            var role = userDb.Username == "risto" ? Roles.Admin : Roles.Viewer;

            //generate JWT token that is used for authentication/authorization
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddHours(1),
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userDb.Username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("id", userDb.Id.ToString()),
                    new Claim("userFullName", $"{userDb.FirstName} {userDb.LastName}")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string tokenString = jwtSecurityTokenHandler.WriteToken(token);

            return tokenString;
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
