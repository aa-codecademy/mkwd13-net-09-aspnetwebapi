using NotesAppDA.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAppDA.Domain
{
	[Table("Notes")]
	public class Note
	{
		[Key] //Primary key
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Text { get; set; }

		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]  //part 1 from the 1-M relationship with Note, where the UserId is the FK
		public User User { get; set; }

		[Required]
		public PriorityEnum Priority { get; set; }

		[Required]
		public TagEnum Tag { get; set; }

		[NotMapped] //states that the property will not be mapped in the table
		public int NoteCount { get; set; }
	}
}
