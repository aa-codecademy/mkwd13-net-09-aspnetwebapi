using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dtos.NoteDtos;
using Avenga.NotesApp.Mappers;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared.CustomExceptions;

namespace Avenga.NotesApp.Services.Implementations
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

        public void AddNote(AddNoteDto addNoteDto)
        {
            //1. Validation
            User userDb = _userRepository.GetById(addNoteDto.UserId);
            if (userDb == null)
            {
                throw new NoteDataException($"User with id {addNoteDto.UserId} does not exist!");
            }

            if (string.IsNullOrEmpty(addNoteDto.Text))
            {
                throw new NoteDataException("Text Field is required!");
            }

            if(addNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Text cannot contain more than 100 characters");
            }

            //2. Map to domain model
            Note newNote = addNoteDto.ToNote();
            newNote.User = userDb;

            //3. Add new note to the db
            _noteRepository.Add(newNote);
        }

        public void DeleteNote(int id) 
        {
            Note noteDb = _noteRepository.GetById(id);
            if(noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id {id} was not found");
            }

            _noteRepository.Delete(noteDb);
        }

        public List<NoteDto> GetAllNotes()
        {
            var notesDb =_noteRepository.GetAll();
            return notesDb.Select(x => x.ToNoteDto()).ToList();
        }

        public NoteDto GetById(int id)
        {
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null) throw new NoteNotFoundException($" Note with id {id} was not found");
            NoteDto noteDto = noteDb.ToNoteDto();
            return noteDto;
        }

        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            //1.Validations
            Note noteDb = _noteRepository.GetById(updateNoteDto.Id);
            if (noteDb == null) throw new NoteNotFoundException($"Note with id {updateNoteDto.Id} was not found");
            User userDb = _userRepository.GetById(updateNoteDto.UserId);
            if (userDb == null) throw new NoteDataException($"User with id {updateNoteDto.Id} does not exist!");
            if (string.IsNullOrEmpty(updateNoteDto.Text)) throw new NoteDataException("Text field is required");
            if (updateNoteDto.Text.Length > 100) throw new NoteDataException("Text cannot contain more than 100 characters!");

            //2.Update - we must update the object that we read from the db
            noteDb.Text = updateNoteDto.Text;
            noteDb.Priority = updateNoteDto.Priority;
            noteDb.Tag = updateNoteDto.Tag;
            noteDb.UserId = updateNoteDto.UserId;
            noteDb.User = userDb;
            _noteRepository.Update(noteDb);

        }
    }
}
