using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess
{
	public class NotesAppDbContext : DbContext
	{
		public NotesAppDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Note> Notes { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); //with this we take the impl from the base class and we can add our extra logic here

			modelBuilder.Entity<Note>() //we take the entity
				.Property(x => x.Text) //we take the property from the entity 
				.IsRequired() //we say that this property is required
				.HasMaxLength(100); //and we say that this propery can have max length of 100 chars

			modelBuilder.Entity<Note>()
				.Property(x => x.Priority)
				.IsRequired();

			modelBuilder.Entity<Note>()
				.Property(x => x.Tag)
				.IsRequired();

			//relation
			modelBuilder.Entity<Note>() //the entity Note
				.HasOne(x => x.User) //has one user
				.WithMany(x => x.Notes) //the user has many notes
				.HasForeignKey(x => x.UserId); //the FK that connects note to user is the userId

			//we can configure this relation either way 
			//modelBuilder.Entity<User>() //the entity User
			//	.HasMany(x => x.Notes) //has many notes
			//	.WithOne(x => x.User) //has one user
			//	.HasForeignKey(x => x.UserId); //the FK that connects note and user is the userId

			modelBuilder.Entity<User>()
				.Property(x => x.FirstName)
				.HasMaxLength(50);

			modelBuilder.Entity<User>()
				.Property(x => x.LastName)
				.HasMaxLength(50);

			modelBuilder.Entity<User>()
				.Property(x => x.Username)
				.IsRequired()
				.HasMaxLength(30);

			modelBuilder.Entity<User>()
				.Ignore(x => x.Age); //ignore -> not mapped
		}
	}
}
