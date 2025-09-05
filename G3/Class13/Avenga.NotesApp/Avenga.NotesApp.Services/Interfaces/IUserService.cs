using Avenga.NotesApp.Dto.Users;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginDto loginDto);
    }
}
