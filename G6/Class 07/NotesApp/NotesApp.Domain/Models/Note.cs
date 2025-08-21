using NotesApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Domain.Models
{
	public class Note : BaseEntity
	{
		public string Text { get; set; }
		public PriorityEnum Priority { get; set; }
		public TagEnum Tag { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }	
	}
}
