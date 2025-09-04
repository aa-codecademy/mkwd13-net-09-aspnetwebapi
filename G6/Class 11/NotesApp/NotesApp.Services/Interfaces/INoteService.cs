using NotesApp.DTOs;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes(FilterDto filter, int userId);
        NoteDto GetById(int id, int userId);
        void AddNote(AddNoteDto note, int userId);
        void DeleteById(int id);
    }
}
