using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.DataAccess.Implementations
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _moviesDbContext;
        public MovieRepository(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }
        public void Add(Movie entity)
        {
            _moviesDbContext.Movies.Add(entity);
            _moviesDbContext.SaveChanges();
        }

        public void Delete(Movie entity)
        {
            _moviesDbContext.Movies.Remove(entity);
            _moviesDbContext.SaveChanges();
        }

        public List<Movie> FilterMovies(int? year, GenreEnum? genre)
        {
            if(year == null)
            {
                List<Movie> movieDb = _moviesDbContext.Movies.Where(x=>x.Genre == (GenreEnum)genre).ToList();
                return movieDb;
            }

            if(genre == null)
            {
                List<Movie> movieDb = _moviesDbContext.Movies.Where(x=>x.Year == year).ToList();
                return movieDb;
            }

            List<Movie> movies = _moviesDbContext.Movies.Where(x=> 
                                                        x.Year == year && 
                                                        x.Genre == (GenreEnum)genre
                                                        ).ToList();

            return movies;
        }

        public List<Movie> GetAll()
        {
            return _moviesDbContext.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _moviesDbContext.Movies.SingleOrDefault(x => x.Id == id);
        }

        public void Update(Movie entity)
        {
            _moviesDbContext.Movies.Update(entity);
            _moviesDbContext.SaveChanges();
        }
    }
}
