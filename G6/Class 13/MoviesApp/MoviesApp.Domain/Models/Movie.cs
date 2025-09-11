using MoviesApp.Domain.Enums;

namespace MoviesApp.Domain.Models
{
	public class Movie : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int Year { get; set; }
		public GenreEnum Genre {get; set;}
		public int UserId { get; set;}
		public User User { get; set; }
	}
}
