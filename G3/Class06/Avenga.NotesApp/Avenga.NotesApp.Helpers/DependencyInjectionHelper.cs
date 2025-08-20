

using Avenga.NotesApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Avenga.NotesApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services) 
        {
            services.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer("Server=DANILO\\SQLEXPRESS;Database=NotesApp;Trusted_Connection=True;TrustServerCertificate=True;"));
        }
    }
}
