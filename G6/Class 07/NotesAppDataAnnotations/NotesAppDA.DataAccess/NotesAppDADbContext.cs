using Microsoft.EntityFrameworkCore;
using NotesAppDA.Domain;

namespace NotesAppDA.DataAccess
{
	public class NotesAppDADbContext : DbContext
	{
		public NotesAppDADbContext(DbContextOptions options) : base(options) { }

		public DbSet<Note> Notes { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
