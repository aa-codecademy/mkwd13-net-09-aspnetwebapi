using Avenga.DataAnnotations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Avenga.DataAnnotations.DataAccess
{
    public class DataAnnotationsDbContext : DbContext
    {
        public DataAnnotationsDbContext(DbContextOptions options) : base(options)
        {

        }
        
        //Reprezentacija na tabelite vo databaza sto ke se kreiraat
        public DbSet<Note> Notes { get; set; } // Tabela vo baza Notes
        public DbSet<User> Users { get; set; } // Tabela vo baza Users
    }
}
