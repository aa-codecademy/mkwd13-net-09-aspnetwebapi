using Avenga.MovieApp.Domain.Models;
using Avenga.MovieApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.Mappers
{
    public static class MovieMapper
    {
        public static Movie ToMovie(this AddMovieDto addMovieDto)
        {
            return new Movie
            {
                Year = addMovieDto.Year,
                Title = addMovieDto.Title,
                Genre = addMovieDto.Genre,
                Description = addMovieDto.Description
            };
        }

        public static MovieDto ToMovieDto(this Movie movie) 
        {
            return new MovieDto
            {
                Id = movie.Id,
                Year = movie.Year,
                Title = movie.Title,
                Genre = movie.Genre,
                Description = movie.Description
            };
        }
    }
}
