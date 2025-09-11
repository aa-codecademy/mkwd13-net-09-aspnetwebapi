using MoviesApp.Domain.Enums;
using MoviesApp.Domain.Models;

namespace MoviesApp.DataAccess.Interfaces
{
	public interface IMovieRepository : IRepository<Movie>
	{
		List<Movie> FilterMovies(int? year, GenreEnum? genre);
	}
}
