using Avenga.NotesApp.Dtos.NoteDtos;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes();
        NoteDto GetById(int id);
        void AddNote(AddNoteDto note);
        void UpdateNote(UpdateNoteDto note);
        void DeleteNote(int id);
    }
}
