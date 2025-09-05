using Avenga.NotesApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes();
        NoteDto GetByIdNote(int id);
        void AddNote(AddNoteDto addNoteDto);
        void UpdateNote(UpdateNoteDto updateNoteDto);
        void DeleteNote(int id);
    }
}
