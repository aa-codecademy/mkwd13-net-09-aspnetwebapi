using Avenga.FluentAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Avenga.FluentAPI.Helpers
{
    public static class DIHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<FluentApiDbContext>(x =>
            x.UseSqlServer("Server=.;Database=FluentApiDb;Trusted_Connection=True;TrustServerCertificate=True"));
        }
    }
}
