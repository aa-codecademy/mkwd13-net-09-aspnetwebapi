using Avenga.MovieApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.MovieApp.Domain.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
