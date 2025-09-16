using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Dtos;
using Avenga.MovieApp.Services.Implementations;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using MovieApp.Tests.FakeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Tests
{
    [TestClass]
    public class MovieTests
    {
        private IMovieService _movieService;

        [TestInitialize]
        public void Setup()
        {
            _movieService = new MovieService(new FakeMovieRepository());
        }

        #region AddMovie Tests
        [TestMethod]
        public void AddMovie_ValidInput_ShouldAddMovie()
        {
            var dto = new AddMovieDto
            {
                Title = "New Movie",
                Description = "Description",
                Year = 2023,
                Genre = GenreEnum.Action
            };

            int userId = 1;

            _movieService.AddMovie(dto, userId);

            var movies = _movieService.GetAllMovies(userId);
            Assert.IsTrue(movies.Any(x=>x.Title == "New Movie" && x.Year == 2023));
        }

        [TestMethod]
        [ExpectedException(typeof(MovieException))]
        public void AddMovie_EmptyTitle_ShouldThrowException()
        {
            var dto = new AddMovieDto
            {
                Title = "",
                Description = "Description",
                Year = 2000,
                Genre = GenreEnum.Romance
            };
            int userId = 1;

            _movieService.AddMovie(dto, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(MovieException))]
        public void AddMovie_DescriptionTooLong_ShouldThrowException()
        {
            var dto = new AddMovieDto
            {
                Title = "Title",
                Description = new string('a', 251),
                Year = 2010,
                Genre = GenreEnum.Romance
            };
            int userId = 1;

            _movieService.AddMovie(dto, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(MovieException))]
        public void AddMovie_NegativeYear_ShouldThrowException()
        {
            var dto = new AddMovieDto
            {
                Title = "Title",
                Description = "Description",
                Year = -10,
                Genre = GenreEnum.Romance
            };
            int userId = 1;

            _movieService.AddMovie(dto, userId);
        }


        #endregion

        #region DeleteMovie Tests
        [TestMethod]
        public void DeleteMovie_ValidId_ShouldRemoveMovie()
        {
            int movieId = 1;
            int userId = 1;
            _movieService.DeleteMovie(movieId);

            var movie = _movieService.GetAllMovies(userId).SingleOrDefault(x => x.Id == movieId);
            Assert.IsNull(movie);
        }

        [TestMethod]
        [ExpectedException(typeof(MovieException))]
        public void DeleteMovie_NegativeId_ShouldThrowException()
        {
            int movieId = -12;
            _movieService.DeleteMovie(movieId);
        }

        [TestMethod]
        [ExpectedException(typeof(MovieNotFoundException))]
        public void DeleteMovie_InvalidId_ShouldThrowException()
        {
            int movieId = 4;
            _movieService.DeleteMovie(movieId);
        }

        #endregion


        #region FilterMovie Tests
        [TestMethod]
        public void FilterMovies_ByYear_ShouldReturnCorrectMovies()
        {
            int year = 2020;

            var filtered = _movieService.FilterMovies(year, null);
            Assert.IsTrue(filtered.Any(x=>x.Year == 2020));
        }

        [TestMethod]
        public void FilterMovies_ByYearAndGenre_ShouldReturnCorrectMovies()
        {
            int year = 2020;
            var genre = GenreEnum.Action;
            var filtered = _movieService.FilterMovies(year, genre);
            Assert.AreEqual("Movie 1", filtered[0].Title);
            Assert.AreEqual(1, filtered.Count);
        }

        [TestMethod]
        [ExpectedException(typeof (MovieException))]
        public void FilterMovies_InvalidGenre_ShouldThrowException()
        {
            var genre = (GenreEnum)999;
            var filtered = _movieService.FilterMovies(null, genre);
        }
        #endregion
    }
}
