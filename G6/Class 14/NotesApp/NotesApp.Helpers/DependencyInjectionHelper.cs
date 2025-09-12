using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.DataAccess;


namespace NotesApp.Helpers
{
	public static class DependencyInjectionHelper
	{
		public static void InjectDbContext(IServiceCollection services)
		{
			services.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer("Server=.\\SQLExpress;Database=AANotesApp;Trusted_Connection=True;TrustServerCertificate=True"));
		}
	}
}
