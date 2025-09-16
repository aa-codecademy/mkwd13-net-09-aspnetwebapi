using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dtos.UserDtos;
using Avenga.NotesApp.Mappers;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace Avenga.NotesApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public void DeleteUser(int id)
        {
            //var user = _userRepository.GetById(id);
            User user = _userRepository.GetById(id); // better, use when you can
            if (user == null) throw new UserNotFoundException($"User with id {id} was not found!");
            _userRepository.Delete(user);
        }

        public List<UserDto> GetAllUsers()
        {
            //first we are getting all the users from our database
            List<User> users = _userRepository.GetAll();
            //if (users.Count == 0) throw new UserNotFoundException("No Users in our database!");
            // No point in throwing exception when there is no error, just empty list

            List<UserDto> userDtos = new List<UserDto>();
            foreach (User user in users)
            {
                UserDto userDto = user.ToUserDto();
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public UserDto GetById(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null) throw new UserNotFoundException($"User with id {id} was not found!");
            UserDto userDto = user.ToUserDto();
            return userDto;
        }

        public string LoginUser(LoginUserDto loginUserDto)
        {

            //1. validation
            if (string.IsNullOrEmpty(loginUserDto.UserName) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new UserDataException("Username and password are required fields!");
            }

            //2. hash the password
            //MD5 hash algorithm

            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            // Test123 => 4856723845
            byte[] passwordBytes = Encoding.ASCII.GetBytes(loginUserDto.Password);

            //get the bytes of the has string 4856723845 => 2342324
            byte[] hashBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            //get the hash as string 2342324 => 832ubfv832
            string hash = Encoding.ASCII.GetString(hashBytes);

            //3. trying to find if a user with given username and password(which is hashed) exists in DB
            User userDb = _userRepository.LoginUser(loginUserDto.UserName, hash);
            if(userDb == null)
            {
                throw new UserNotFoundException("User not found!");
            }

            //4. GENERATE THE JWT TOKEN
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our very very secret secret key!");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(1), //the token will be valid for one hour upon creation
                //signature configuration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                SecurityAlgorithms.HmacSha256Signature),
                //payload
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, userDb.Username),
                        new Claim("userFullName", $"{userDb.FirstName} {userDb.LastName}")
                        //new Claim("role", userDb.Role)
                    }
                )
            }; 

            //generate the token
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            //1. Validate
            ValidateUser(registerUserDto);

            //2. hash the password
            //MD5 hash algorithm

            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            // Test123 => 4856723845
            byte[] passwordBytes = Encoding.ASCII.GetBytes(registerUserDto.Password);

            //get the bytes of the has string 4856723845 => 2342324
            byte[] hashBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            //get the hash as string 2342324 => 832ubfv832
            string hash = Encoding.ASCII.GetString(hashBytes);

            //3. Create the User
            User user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Username = registerUserDto.Username,
                Password = hash, // => we will save the hashed password in our Db, not the inputed one by the user
            };
            _userRepository.Add(user);
        }

        public void UpdateUser(UpdateUserDto updateUserDto)
        {
            User user = _userRepository.GetById(updateUserDto.Id);
            if (user == null) throw new UserNotFoundException($"User with id {updateUserDto.Id} was not found!");
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Username = updateUserDto.Username;
            _userRepository.Update(user);
        }

        private void ValidateUser(RegisterUserDto registerUserDto)
        {
            if(string.IsNullOrEmpty(registerUserDto.Username) || string.IsNullOrEmpty(registerUserDto.Password) || string.IsNullOrEmpty(registerUserDto.ConfirmPassword))
            {
                throw new UserDataException("Username and password are required fields!");
            }
            if(registerUserDto.Username.Length > 30)
            {
                throw new UserDataException("Username: maximum lenght is 30 characters");
            }
            if(!string.IsNullOrEmpty(registerUserDto.FirstName) && registerUserDto.FirstName.Length > 50)
            {
                throw new UserDataException("FirstName: maximum lenght is 50 characters");
            }
            if (!string.IsNullOrEmpty(registerUserDto.LastName) && registerUserDto.LastName.Length > 50)
            {
                throw new UserDataException("LastName: maximum lenght is 50 characters");
            }
            if(registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new UserDataException("Passwords must match!");
            }

            var userDb = _userRepository.GetUserByUsername(registerUserDto.Username);
            if(userDb != null)
            {
                throw new UserDataException("Username is already in use, try another one");
            }
        }
    }
}
