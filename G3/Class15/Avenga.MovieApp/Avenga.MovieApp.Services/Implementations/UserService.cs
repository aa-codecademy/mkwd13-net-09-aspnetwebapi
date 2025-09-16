using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Models;
using Avenga.MovieApp.Dtos.UserDto;
using Avenga.MovieApp.Mappers;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace Avenga.MovieApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public UserDto LoginUser(LoginUserDto loginUser)
        {
            //1. HASHING PASSWORD
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] bytePassword = Encoding.ASCII.GetBytes(loginUser.Password);
            byte[] bytePasswordHashed = mD5CryptoServiceProvider.ComputeHash(bytePassword);
            string hashedPassword = Encoding.ASCII.GetString(bytePasswordHashed);

            //2. VALIDATING THE USERNAME AND PASSWORD
            User user = _userRepository.GetAll().SingleOrDefault(x=>x.Username == loginUser.Username && x.Password == hashedPassword);

            if (user == null) return null;

            //3.AUTHENTICATION (CREATING A BEARER TOKEN)
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes("Our very secret secret secret secret secret key");
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }
                    ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = securityTokenHandler.CreateToken(securityTokenDescriptor);
            Log.Information($"Succ created token with username {loginUser.Username}");

            //4. MAPPING FROM MODEL TO DTO

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Token = securityTokenHandler.WriteToken(token),
                MovieList = user.MovieList.Select(x=> x.ToMovieDto()).ToList()
            };

            return userDto;
        }

        public void RegisterUser(RegisterUserDto registerUser)
        {
            //1. VALIDATIONS
            ValidRegisterUser(registerUser);

            //2. HASHING PASSWORD
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            //byte[] bytePassword = Encoding.ASCII.GetBytes(registerUser.Password);
            //byte[] bytesHahedPassword = mD5CryptoServiceProvider.ComputeHash(bytePassword);
            byte[] bytesHashedPassword = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(registerUser.Password));
            string hashedPassword = Encoding.ASCII.GetString(bytesHashedPassword);


            //3. MAPPING FROM DTO TO MODEL
            User user = new User()
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Username = registerUser.Username,
                Password = hashedPassword
            };

            //4. ADD TO DATABASE
            _userRepository.Add(user);
        }

        private void ValidRegisterUser(RegisterUserDto registerUser)
        {
            if (string.IsNullOrEmpty(registerUser.FirstName))
            {
                throw new UserException(null, registerUser.Username, "First Name is required!");
            }
            if (string.IsNullOrEmpty(registerUser.LastName))
            {
                throw new UserException(null, registerUser.Username, "Last Name is required!");
            }
            if (string.IsNullOrEmpty(registerUser.Username))
            {
                throw new UserException(null, registerUser.Username, "Username is required!");
            }
            if (!ValidUsername(registerUser.Username))
            {
                throw new UserException(null, registerUser.Username, "Username is alredy in use!");
            }
            if (string.IsNullOrEmpty(registerUser.Password))
            {
                throw new UserException(null, registerUser.Username, "Password is required!");
            }
            if (string.IsNullOrEmpty(registerUser.ConfirmPassword))
            {
                throw new UserException(null, registerUser.Username, "ConfirmPassword is required!");
            }
            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                throw new UserException(null, registerUser.Username, "Passwords did not match!");
            }
        }

        private bool ValidUsername(string username)
        {
            return _userRepository.GetAll().All(x=> x.Username != username);
        }
    }
}
