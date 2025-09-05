using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.DataAccess.AdoImplementations;
using Avenga.NotesApp.DataAccess.DapperRepository;
using Avenga.NotesApp.DataAccess.Implementations;
using Avenga.NotesApp.DataAccess.Interfaces;
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
        public static void InjectDbContext(IServiceCollection service, string connectionString)
        {
            service.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer(connectionString));
        }

        public static void InjectRepositories(IServiceCollection service)
        {
            service.AddTransient<IRepository<Note>, NoteRepository>();
            service.AddTransient<IUserRepository, UserRepository>();
        }

        public static void InjectAdoRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<Note>>(x=> new NoteAdoRepository(connectionString));
        }

        public static void InjectDapperRepositories(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<Note>>(x=> new NoteDapperRepository(connectionString));
        }


        public static void InjectServices(IServiceCollection services) 
        {
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFruitService, FruitService>();
        }

    }
}
