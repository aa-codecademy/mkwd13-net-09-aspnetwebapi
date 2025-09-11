using MoviesApp.Domain.Enums;

namespace MoviesApp.Dtos
{
	public class AddMovieDto
	{
		public string Title { get; set; }
		public string Description { get; set; }	
		public int Year { get; set; }
		public GenreEnum Genre { get; set; }
	}
}
