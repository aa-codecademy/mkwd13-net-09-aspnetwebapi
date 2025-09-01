using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies(int userId);
        List<MovieDto> FilterMovies(int? year, GenreEnum? genre);
        MovieDto GetMovieById(int id);
        void AddMovie(AddMovieDto addMovieDto, int userId);
        void UpdateMovie(UpdateMovieDto updateMovieDto);
        void DeleteMovie(int id);
    }
}
