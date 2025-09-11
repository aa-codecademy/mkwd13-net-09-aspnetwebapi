using MoviesApp.Domain.Enums;

namespace MoviesApp.Domain.Models
{
	public class User : BaseEntity
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public GenreEnum FavouriteGenre { get; set; }

		public List<Movie> MoviesList { get; set; }
	}
}
