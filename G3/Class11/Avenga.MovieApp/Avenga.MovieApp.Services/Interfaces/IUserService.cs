using Avenga.MovieApp.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.Services.Interfaces
{
    public interface IUserService
    {
        UserDto LoginUser(LoginUserDto loginUser);
        void RegisterUser (RegisterUserDto registerUser);
    }
}
