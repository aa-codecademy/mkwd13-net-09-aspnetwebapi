using MoviesApp.DataAccess.Interfaces;
using MoviesApp.Dtos;
using MoviesApp.Services.Interfaces;

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
			throw new NotImplementedException();
		}

		public void RegisterUser(RegisterUserDto registerUserDto)
		{
			throw new NotImplementedException();
		}
	}
}
