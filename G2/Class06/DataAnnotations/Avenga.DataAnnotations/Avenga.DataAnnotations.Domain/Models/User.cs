using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avenga.DataAnnotations.Domain.Models
{
    [Table("Users")]
    public class User
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // The db will manage the ID's
        public int Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [InverseProperty("User")] // This Notes collection corresponds to the User navigaional property inside the Nite entity ili so dr zborovi match the navigaion property name in Note.
        public List<Note> Notes { get; set; }
    }
}
