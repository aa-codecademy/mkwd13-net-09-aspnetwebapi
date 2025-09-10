using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto ToNoteDto(this Note note)
        {
            return new NoteDto
            {
                Id = note.Id,
                Tag = note.Tag,
                Priority = note.Priority,
                Text = note.Text,
                UserFullName = $"{note.User.FirstName} {note.User.LastName}"
            };
        }

        public static Note ToNote(this AddNoteDto addNote)
        {
            return new Note()
            {
                Tag = addNote.Tag,
                Priority = addNote.Priority,
                Text = addNote.Text
            };
        }
    }
}
