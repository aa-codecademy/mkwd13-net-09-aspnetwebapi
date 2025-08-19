using Avenga.DataAnnotations.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avenga.DataAnnotations.Domain.Models
{
    [Table("Notes")] // soodvetnata tabela ke bide Notes
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] // not null
        [MaxLength(100)]
        public string Text { get; set; }
        [Required]
        public Priority Priority { get; set; }
        [Required]
        public Tag Tag { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
