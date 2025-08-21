using Avenga.DataAnnotations.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Avenga.DataAnnotations.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<DataAnnotationsDbContext>(x =>
            x.UseSqlServer("Server=.;Database=DataAnnotationsDb;Trusted_Connection=True;TrustServerCertificate=True"));
        }
    }
}
