using MoviesApp.Domain.Enums;
using MoviesApp.Dtos;

namespace MoviesApp.Services.Interfaces
{
	public interface IMovieService
	{
		List<MovieDto> GetAllMovies(int userId);
		MovieDto GetMovieById(int id);
		List<MovieDto> FilterMovies(int? year, GenreEnum? genre);
		void DeleteMovie(int id);
		void AddMovie(AddMovieDto addMovieDto, int userId); //the userId will be the id of the logged in user
		void UpdateMovie(UpdateMovieDto updateMovieDto);
	}
}
