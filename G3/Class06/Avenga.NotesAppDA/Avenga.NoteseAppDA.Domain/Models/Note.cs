using Avenga.NoteseAppDA.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NoteseAppDA.Domain.Models
{
    [Table("Notes")]
    public class Note
    {
        [Key] //PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] //not null
        [MinLength(5)]
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
