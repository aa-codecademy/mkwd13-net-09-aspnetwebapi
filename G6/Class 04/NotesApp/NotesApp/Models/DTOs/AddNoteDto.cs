using NotesApp.Models.Enums;

namespace NotesApp.Models.DTOs
{
	public class AddNoteDto
	{
		public string Text { get; set; }
		public PriorityEnum Priority { get; set; }	
		public int UserId { get; set; }
		public List<int> TagIds { get; set; }
	}
}
