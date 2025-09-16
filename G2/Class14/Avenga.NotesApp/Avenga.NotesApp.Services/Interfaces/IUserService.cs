using Avenga.NotesApp.Dtos.UserDtos;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
        List<UserDto> GetAllUsers();
        UserDto GetById(int id);
        void DeleteUser(int id);
        void UpdateUser(UpdateUserDto updateUserDto);
    }
}
