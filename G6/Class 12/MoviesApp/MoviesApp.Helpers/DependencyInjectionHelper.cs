using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApp.DataAccess;
using MoviesApp.DataAccess.Implementations;
using MoviesApp.DataAccess.Interfaces;

namespace MoviesApp.Helpers
{
	public static class DependencyInjectionHelper
	{
		public static void InjectDbContext(IServiceCollection services)
		{
			services.AddDbContext<MoviesDbContext>(x => x.UseSqlServer("Server=.\\SQLExpress;Database=MoviesApp;Trusted_Connection=True;TrustServerCertificate=True"));
		}

		public static void InjectRepositories(IServiceCollection services)
		{
			services.AddTransient<IMovieRepository, MovieRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
		}
	}
}
