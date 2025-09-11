using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.DataAccess.Implementations;
using Avenga.NotesApp.DataAccess.Interfaces;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Mappers;
using Avenga.NotesApp.Services.Interfaces;
using Avenga.NotesApp.Shared;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IUserRepository _userRepository;

        public NoteService(IRepository<Note> noteRepository, IUserRepository userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public void AddNote(AddNoteDto addNoteDto, int userId)
        {
            //1. validation
            User userDb = _userRepository.GetById(userId);
            if (userDb == null) 
            {
                throw new NoteDataException($"User with id {userId} does not exist!!!");
            }
            if (string.IsNullOrEmpty(addNoteDto.Text))
            {
                throw new NoteDataException("Text is required field!");
            }
            if(addNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Text can not contain more than 100 characters!");
            }

            //2. map to domain model
            Note newNote = addNoteDto.ToNote();
            newNote.User = userDb;
            newNote.UserId = userId;

            //3. add to db
            _noteRepository.Add(newNote);
        }

        public void DeleteNote(int id)
        {
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id {id} was not found!");
            }

            _noteRepository.Delete(noteDb);
        }

        public List<NoteDto> GetAllNotes(int userId)
        {
            List<Note> noteDb = _noteRepository.GetAll();
            return noteDb.Where(x=> x.UserId == userId).Select(x=>x.ToNoteDto()).ToList();
        }

        public NoteDto GetByIdNote(int id)
        {
            Note noteDb = _noteRepository.GetById(id);
            if(noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id {id} was not found!");
            }
            NoteDto noteDto = noteDb.ToNoteDto();
            return noteDto;
        }

        public void UpdateNote(UpdateNoteDto updateNoteDto, int userId)
        {
            //1. validation
            Note noteDb = _noteRepository.GetById(updateNoteDto.Id);
            if(noteDb == null)
            {
                throw new NoteNotFoundException($"Note with id {updateNoteDto.Id} was not found!");
            }

            User userDb = _userRepository.GetById(userId);
            if (userDb == null) 
            {
                throw new NoteDataException($"User with id {userId} does not exist!");
            }

            if (string.IsNullOrEmpty(updateNoteDto.Text)) 
            {
                throw new NoteDataException("Text is required field!");
            }

            if (updateNoteDto.Text.Length > 100) 
            {
                throw new NoteDataException("Text can not contain more than 100 characters!");
            }

            //2. update
            noteDb.Text = updateNoteDto.Text;
            noteDb.Priority = updateNoteDto.Priority;
            noteDb.Tag = updateNoteDto.Tag;
            noteDb.UserId = userId;
            noteDb.User = userDb;

            _noteRepository.Update(noteDb);
        }
    }
}
