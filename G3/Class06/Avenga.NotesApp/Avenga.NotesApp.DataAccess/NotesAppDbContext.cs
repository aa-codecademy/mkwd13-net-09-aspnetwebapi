using Avenga.NotesApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Avenga.NotesApp.DataAccess
{
    public class NotesAppDbContext : DbContext
    {
        public NotesAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Note


            //User

            //Seed
        }


    }
}
