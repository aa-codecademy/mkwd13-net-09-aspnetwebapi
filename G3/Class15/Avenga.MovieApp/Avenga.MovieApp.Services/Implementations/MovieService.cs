using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Domain.Models;
using Avenga.MovieApp.Dtos;
using Avenga.MovieApp.Mappers;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;

namespace Avenga.MovieApp.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository; //DI
        }
        public void AddMovie(AddMovieDto addMovieDto, int userId)
        {
            if (string.IsNullOrEmpty(addMovieDto.Title))
            {
                throw new MovieException("Title must not be empty");
            }
            if (addMovieDto.Description.Length > 250)
            {
                throw new MovieException("Description can not be longer than 250 characters");
            }
            if (addMovieDto.Year <= 0)
            {
                throw new MovieException("Year can not have negative value");
            }

            Movie newMovie = addMovieDto.ToMovie();
            newMovie.UserId = userId;

            _movieRepository.Add(newMovie);
        }

        public void DeleteMovie(int id)
        {
            if (id < 0)
            {
                throw new MovieException("The id can not be negative!");
            }

            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new MovieNotFoundException("Movie was not found!");
            }
            _movieRepository.Delete(movieDb);
        }

        public List<MovieDto> FilterMovies(int? year, GenreEnum? genre)
        {
            if (genre.HasValue)
            {
                var enumValues = Enum.GetValues(typeof(GenreEnum)).Cast<GenreEnum>().ToList();
                if (!enumValues.Contains(genre.Value)) 
                {
                    throw new MovieException("Invalid genre value");
                }
            }

            return _movieRepository.FilterMovies(year, genre).Select(x=>x.ToMovieDto()).ToList();
        }

        public List<MovieDto> GetAllMovies(int userId)
        {
            return _movieRepository.GetAll().Where(x => x.UserId == userId).Select(x=> x.ToMovieDto()).ToList();
        }

        public MovieDto GetMovieById(int id)
        {
            if (id <= 0)
            {
                throw new MovieException("The id can not be negative!");
            }
            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new MovieNotFoundException("Movie was not found");
            }
            return movieDb.ToMovieDto();
        }

        public void UpdateMovie(UpdateMovieDto updateMovieDto)
        {
            Movie movieDb = _movieRepository.GetById(updateMovieDto.Id);
            if (movieDb == null)
                throw new MovieNotFoundException("Resource not found");

            if (string.IsNullOrEmpty(updateMovieDto.Title))
            {
                throw new MovieException("Text must not be empty");
            }
            if (updateMovieDto.Year <= 0)
            {
                throw new MovieException("Year must not be negative");
            }
            if (updateMovieDto.Description.Length > 250)
            {
                throw new MovieException("Description can not be longer than 250 characters");
            }

            movieDb.Year = updateMovieDto.Year;
            movieDb.Title = updateMovieDto.Title;
            movieDb.Description = updateMovieDto.Description;
            movieDb.Genre = updateMovieDto.Genre;

            _movieRepository.Update(movieDb);
        }
    }
}
