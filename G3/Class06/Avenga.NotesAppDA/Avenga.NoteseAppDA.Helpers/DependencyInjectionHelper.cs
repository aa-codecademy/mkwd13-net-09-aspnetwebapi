using Avenga.NoteseAppDA.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NoteseAppDA.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<NotesAppDbContext>(x=> 
            x.UseSqlServer("Server=DANILO\\SQLEXPRESS;Database=NoteAppDA;Trusted_Connection=True;TrustServerCertificate=True;")
            );
        }
    }
}
