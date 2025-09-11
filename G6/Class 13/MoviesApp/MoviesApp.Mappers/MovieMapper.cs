using MoviesApp.Domain.Models;
using MoviesApp.Dtos;

namespace MoviesApp.Mappers
{
	public static class MovieMapper
	{
		//from domain class to movieDto
		public static MovieDto ToMovieDto(this Movie movie) //extension method
		{
			return new MovieDto
			{
				Year = movie.Year,
				Description = movie.Description,
				Title = movie.Title,
				Genre = movie.Genre
			};
		}

		//from dto to domain
		public static Movie ToMovie(this AddMovieDto addMovie)
		{
			return new Movie
			{
				Year = addMovie.Year,
				Description = addMovie.Description,
				Title = addMovie.Title,
				Genre = addMovie.Genre
			};
		}
	}
}
