using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesAppDA.DataAccess;

namespace NotesAppDA.Helpers
{
	public static class DependencyInjectionHelper
	{
		public static void InjectDbContext(IServiceCollection services)
		{
			services.AddDbContext<NotesAppDADbContext>(x =>
			x.UseSqlServer("Server=.\\SQLExpress;Database=AANotesDataAnnotations;Trusted_Connection=True;TrustServerCertificate=True"));
		}
	}
}
