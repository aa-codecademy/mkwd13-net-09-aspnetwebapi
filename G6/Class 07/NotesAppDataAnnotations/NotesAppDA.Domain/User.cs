using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAppDA.Domain
{
	[Table("Users")]
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id {  get; set; }

		[MaxLength(50)]
		public string FirstName { get; set; }

		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(30)]
		public string Username { get; set; }

		[InverseProperty("User")] //it states the other end of the relationship 
		public List<Note> Notes { get; set; } //M-part of the 1-M relationship with User
	}
}
