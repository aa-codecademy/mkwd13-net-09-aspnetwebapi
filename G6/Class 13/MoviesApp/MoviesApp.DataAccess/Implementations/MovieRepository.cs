using Microsoft.EntityFrameworkCore;
using MoviesApp.DataAccess.Interfaces;
using MoviesApp.Domain.Enums;
using MoviesApp.Domain.Models;

namespace MoviesApp.DataAccess.Implementations
{
	public class MovieRepository : IMovieRepository
	{
		private readonly MoviesDbContext _dbContext;
		public MovieRepository(MoviesDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(Movie entity)
		{
			_dbContext.Movies.Add(entity);
			_dbContext.SaveChanges();
		}

		public void Delete(Movie entity)
		{
			_dbContext.Movies.Remove(entity);
			_dbContext.SaveChanges();	
		}

		public List<Movie> FilterMovies(int? year, GenreEnum? genre)
		{
			//if both year and genre are null
			if(year == null && genre == null)
			{
				return _dbContext.Movies.Include(x => x.User).ToList(); //GetAll or we can throw an exception that we need at least one filter
			}

			//if year is null and genre is not null
			if(year == null)
			{
				List<Movie> moviesDb = _dbContext.Movies.Include(x => x.User).Where(x => x.Genre == genre).ToList();
				return moviesDb;
			}

			//if genre is null and year is not null
			if(genre == null)
			{
				List<Movie> moviesDb = _dbContext.Movies.Include(x => x.User).Where(x => x.Year == year).ToList();
				return moviesDb;
			}

			//if both are not null
			List<Movie> movies = _dbContext.Movies.Include(x => x.User).Where(x => x.Year == year && x.Genre == genre).ToList();
			return movies;
		}

		public List<Movie> GetAll()
		{
			return _dbContext.Movies.Include(x => x.User).ToList();
		}

		public Movie GetById(int id)
		{
			return _dbContext.Movies.Include(x => x.User).FirstOrDefault(x => x.Id == id);
		}

		public void Update(Movie entity)
		{
			_dbContext.Movies.Update(entity);
			_dbContext.SaveChanges();
		}
	}
}
