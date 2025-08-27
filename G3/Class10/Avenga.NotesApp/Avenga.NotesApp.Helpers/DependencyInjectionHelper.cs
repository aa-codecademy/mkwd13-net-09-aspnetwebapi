using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.DataAccess.Implementations;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Services.Implementations;
using Avenga.NotesApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection service)
        {
            service.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer("Server=DANILO\\SQLEXPRESS;Database=NotesAppNew;Trusted_Connection=True;TrustServerCertificate=True;"));
        }

        public static void InjectRepositories(IServiceCollection service)
        {
            service.AddTransient<IRepository<Note>, NoteRepository>();
            service.AddTransient<IRepository<User>, UserRepository>();
        }

        public static void InjectServices(IServiceCollection services) 
        {
            services.AddTransient<INoteService, NoteService>();
        }

    }
}
