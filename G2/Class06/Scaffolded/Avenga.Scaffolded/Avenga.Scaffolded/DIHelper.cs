using Avenga.Scaffolded.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Avenga.Scaffolded
{
    public static class DIHelper 
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<DataAnnotationsDbContext>(x =>
            x.UseSqlServer("Server=.;Database=DataAnnotationsDb;Trusted_Connection=True;TrustServerCertificate=True"));
        }
    }
}
