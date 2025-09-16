using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Domain.Models;

namespace MovieApp.Tests.FakeRepository
{
    public class FakeMovieRepository : IMovieRepository
    {
        private List<Movie> _movies;
        public FakeMovieRepository()
        {
            _movies = new List<Movie>()
            {
                new Movie {Id = 1, Title = "Movie 1", Description = "Movie 1", Year = 2020, Genre = GenreEnum.Action},
                new Movie {Id = 2, Title = "Movie 2", Description = "Movie 2", Year = 2021, Genre = GenreEnum.Comedy},
                new Movie {Id = 3, Title = "Movie 3", Description = "Movie 3", Year = 2020, Genre = GenreEnum.Comedy}
            };
        }

        public void Add(Movie entity)
        {
            _movies.Add(entity);
        }

        public void Delete(Movie entity)
        {
            _movies.Remove(_movies.SingleOrDefault(x=>x.Id == entity.Id));
        }

        public List<Movie> FilterMovies(int? year, GenreEnum? genre)
        {
            if(year == null && genre == null) return _movies;
            if(year == null)
                return _movies.Where(x=>x.Genre == genre).ToList();
            if(genre == null)
                return _movies.Where(x=>x.Year == year).ToList();
            
            return _movies.Where(x=>x.Year == year && x.Genre == genre).ToList();
        }

        public List<Movie> GetAll()
        {
            return _movies;
        }

        public Movie GetById(int id)
        {
            return _movies.SingleOrDefault(x => x.Id == id);
        }

        public void Update(Movie entity)
        {
            var movie = _movies.SingleOrDefault(x => x.Id == entity.Id);
            if (movie != null)
            {
                var index = _movies.IndexOf(movie);
                _movies[index] = movie;
            }
        }
    }
}
