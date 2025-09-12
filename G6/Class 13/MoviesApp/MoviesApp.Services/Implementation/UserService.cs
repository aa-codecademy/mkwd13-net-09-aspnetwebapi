using Microsoft.IdentityModel.Tokens;
using MoviesApp.DataAccess.Interfaces;
using MoviesApp.Domain.Enums;
using MoviesApp.Domain.Models;
using MoviesApp.Dtos;
using MoviesApp.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MoviesApp.Services.Implementation
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public string Login(LoginUserDto loginUserDto)
		{
			if(loginUserDto == null)
			{
				throw new NullReferenceException("Model cannot be null");
			}

			if(string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
			{
				throw new NullReferenceException("Username and password are required fields");
			}

			var hashedPassword = GenerateHash(loginUserDto.Password);
			var userDb = _userRepository.GetAll()
				.FirstOrDefault(x => x.UserName == loginUserDto.Username&& x.Password == hashedPassword);

			if(userDb == null)
			{
				throw new DataException("Wrong username or password");
			}

			//generate JWT token that we will use for authentication/authorization
			JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var secretKey = Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key");

			var role = userDb.UserName == "petko" ? Roles.Admin : Roles.StandardUser;
			SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Expires = DateTime.Now.AddHours(2),
				Subject = new System.Security.Claims.ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, loginUserDto.Username),
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

		public void RegisterUser(RegisterUserDto registerUserDto)
		{
			if(registerUserDto == null)
			{
				throw new NullReferenceException("Model cannot be null");
			}

			if(string.IsNullOrEmpty(registerUserDto.FirstName) || string.IsNullOrEmpty(registerUserDto.LastName))
			{
				throw new NullReferenceException("Firstname and lastname are required fields");
			}

			if (string.IsNullOrEmpty(registerUserDto.Username))
			{
				throw new NullReferenceException("Username is a required field");
			}

			if (string.IsNullOrEmpty(registerUserDto.Password))
			{
				throw new NullReferenceException("Password is a required field");
			}

			if(registerUserDto.Password != registerUserDto.ConfirmPassword)
			{
				throw new DataException("Passwords must match");
			}

			if(_userRepository.GetUserByUsername(registerUserDto.Username) != null)
			{
				throw new DataException("User with that username already exists");
			}

			string strongPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$";

			if (!Regex.IsMatch(registerUserDto.Password, strongPasswordRegex))
			{
				throw new DataException("Password is not a strong password");
			}

			//create new user
			User user = new User
			{
				FirstName = registerUserDto.FirstName,
				LastName = registerUserDto.LastName,
				UserName = registerUserDto.Username,
				Password = GenerateHash(registerUserDto.Password) //we need to hash our password before saving it in the db, we must not save the original string in the db
			};

			_userRepository.Add(user);
		}

		private string GenerateHash(string password)
		{
			using(var md5Hash = MD5.Create())
			{
				var passwordBytes = Encoding.ASCII.GetBytes(password);
				var hashedBytes = md5Hash.ComputeHash(passwordBytes);
				var hashed = Encoding.ASCII.GetString(hashedBytes);

				return hashed;
			}
		}
	}
}
