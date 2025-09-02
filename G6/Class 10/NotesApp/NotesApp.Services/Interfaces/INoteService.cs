using NotesApp.DTOs;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes(FilterDto filter);
        NoteDto GetById(int id);
        void AddNote(AddNoteDto note);
    }
}
