using Microsoft.EntityFrameworkCore;
using MoviesApp.Domain.Models;

namespace MoviesApp.DataAccess
{
	public class MoviesDbContext : DbContext
	{
		public MoviesDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//one-to-many rel between movie and user
			modelBuilder.Entity<Movie>()
				.HasOne(x => x.User)
				.WithMany(x => x.MoviesList)
				.HasForeignKey(x => x.UserId);

			//modelBuilder.Entity<User>()
			//	.HasMany(x => x.MoviesList)
			//	.WithOne(x => x.User)
			//	.HasForeignKey(x => x.UserId);

			modelBuilder.Entity<Movie>()
				.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(200);

			modelBuilder.Entity<Movie>()
				.Property(x => x.Year)
				.IsRequired();

			modelBuilder.Entity<Movie>()
				.Property(x => x.Genre)
				.IsRequired();

			modelBuilder.Entity<Movie>()
				.Property(x => x.Description)
				.HasMaxLength(250);

			modelBuilder.Entity<User>()
				.Property(x => x.UserName)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<User>()
				.Property(x => x.Password)
				.IsRequired();

			modelBuilder.Entity<User>()
				.Property(x => x.FirstName)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Entity<User>()
				.Property(x => x.LastName)
				.IsRequired()
				.HasMaxLength(100);

			base.OnModelCreating(modelBuilder);
		}
	}
}
