using NotesApp.Domain.Enums;

namespace NotesApp.DTOs
{
    public class AddNoteDto
    {
        public string Text { get; set; }
        public PriorityEnum Priority { get; set; }
        public TagEnum Tag { get; set; }
    }
}
