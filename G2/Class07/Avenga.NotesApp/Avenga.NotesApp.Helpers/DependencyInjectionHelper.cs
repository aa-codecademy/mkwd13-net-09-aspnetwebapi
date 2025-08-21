using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.DataAccess.Implementations;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Services.Implementations;
using Avenga.NotesApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Avenga.NotesApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services) 
        {
            services.AddDbContext<NotesAppDbContext>(x =>
            x.UseSqlServer("Server=.;Database=NotesAppDatabase;Trusted_Connection=True;TrustServerCertificate=True"));
        }

        public static void InjectRepositories(IServiceCollection services) 
        {
            services.AddTransient<IRepository<Note>, NoteRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
        }

        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<INoteService, NoteService>();
        }
    }
}
