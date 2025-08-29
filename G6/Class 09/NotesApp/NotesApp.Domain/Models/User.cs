using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Domain.Models
{
	public class User : BaseEntity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string Username { get; set; }

		public List<Note> Notes { get; set;}

		public int? Age { get; set; } //not mapped
	}
}
