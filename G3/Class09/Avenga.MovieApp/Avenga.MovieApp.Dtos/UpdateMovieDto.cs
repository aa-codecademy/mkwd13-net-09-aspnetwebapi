using Avenga.MovieApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.Dtos
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
    }
}
