using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.DataAccess.Implementations;
using Avenga.MovieApp.Services.Implementations;
using Avenga.MovieApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Avenga.MovieApp.Helpers
{
    public static class DependencyInjectionHelpper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<MoviesDbContext>(x => x.UseSqlServer("Server=DANILO\\SQLEXPRESS;Database=MovieAppNew;Trusted_Connection=True;TrustServerCertificate=True"));
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
