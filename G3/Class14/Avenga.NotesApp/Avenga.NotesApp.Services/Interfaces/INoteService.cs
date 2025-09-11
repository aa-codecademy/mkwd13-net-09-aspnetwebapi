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
        List<NoteDto> GetAllNotes(int userId);
        NoteDto GetByIdNote(int id);
        void AddNote(AddNoteDto addNoteDto, int userId);
        void UpdateNote(UpdateNoteDto updateNoteDto, int userId);
        void DeleteNote(int id);
    }
}
