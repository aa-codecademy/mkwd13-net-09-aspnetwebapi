using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Mappers;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementation
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<User> _userRepository;

        public NoteService(IRepository<Note> noteRepository, IRepository<User> userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }

        public void AddNote(AddNoteDto note)
        {
            if(note == null) throw new ArgumentNullException("Model should populated!");

            if (string.IsNullOrEmpty(note.Text)) throw new ArgumentNullException("Text is required");

            if (note.Text.Length > 100) throw new ArgumentException("Text lenght should be max 100 chars");

            //if (note.Tag == null) throw new ArgumentNullException("Tag is required"); //Tag is not nullable, so this condition will be false always

            var user = _userRepository.GetById(note.UserId);

            if (user == null) throw new ArgumentException($"User with id: {note.UserId}, is not found");

            var noteDb = new Note
            {
                Text = note.Text,
                Priority = note.Priority,
                Tag = note.Tag,
                UserId = note.UserId
            };

            _noteRepository.Add(noteDb);
        }

        public List<NoteDto> GetAllNotes()
        {
            var notes = _noteRepository.GetAll();

            var notesDto = notes.Select(x => x.ToDto()).ToList();

            return notesDto;
        }

        public NoteDto GetById(int id)
        {
            var note = _noteRepository.GetById(id);

            return note.ToDto();
        }
    }
}
