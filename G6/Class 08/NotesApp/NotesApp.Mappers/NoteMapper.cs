using NotesApp.Domain.Models;
using NotesApp.DTOs;

namespace NotesApp.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto ToDto(this Note note)
        {
            return new NoteDto
            {
                Id = note.Id,
                Text = note.Text,
                Tag = note.Tag,
                Priority = note.Priority,
                UserFullName = $"{note.User.FirstName} {note.User.LastName}"
            };
        }
    }
}
