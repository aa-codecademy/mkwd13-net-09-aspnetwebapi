using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace Avenga.MovieApp.DataAccess
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions options) : base(options){}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //RELATION
            modelBuilder.Entity<Movie>()
                .HasOne(x => x.User)
                .WithMany(x => x.MovieList)
                .HasForeignKey(x => x.UserId);

            //1.Math Alg for hashing
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            //2.Input password to bytes
            byte[] passwordByte = Encoding.ASCII.GetBytes("admin123");
            //3. hashing the byte
            byte[] hashByte = mD5CryptoServiceProvider.ComputeHash(passwordByte);
            //4. format hash from byte to string
            string hashedPassword = Encoding.ASCII.GetString(hashByte);

            modelBuilder.Entity<User>()
                .HasData(
                    new User()
                    {
                        Id = 1,
                        FirstName = "FNAdmin",
                        LastName = "LNAdmin",
                        Username = "admin",
                        Password = hashedPassword
                    });

            modelBuilder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Year)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(x => x.Description)
                .HasMaxLength(250);



            modelBuilder.Entity<Movie>()
                .HasData(
                new Movie()
                {
                    Id = 1,
                    Title = "Bames Jond 2",
                    Description = "Bames returns for one last mission to save the president from impending doom.",
                    Genre = GenreEnum.Action,
                    Year = 1970,
                    UserId = 1
                });
        }
    }
}
