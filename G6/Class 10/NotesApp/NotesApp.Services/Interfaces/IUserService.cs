using NotesApp.DTOs;

namespace NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string Login(LoginUserDto loginUserDto);
    }
}
