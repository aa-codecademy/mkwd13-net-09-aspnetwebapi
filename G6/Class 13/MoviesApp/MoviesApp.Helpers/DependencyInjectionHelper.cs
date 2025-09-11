using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApp.DataAccess;
using MoviesApp.DataAccess.Implementations;
using MoviesApp.DataAccess.Interfaces;
using MoviesApp.Services.Implementation;
using MoviesApp.Services.Interfaces;

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

		public static void InjectServices(IServiceCollection services)
		{
			services.AddTransient<IMovieService, MovieService>();
			services.AddTransient<IUserService, UserService>();
		}
	}
}
