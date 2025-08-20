using Avenga.NotesApp.DataAccess;
using Avenga.NotesApp.DataAccess.Implementations;
using Avenga.NotesApp.Domain.Models;
using Avenga.NotesApp.Dto;
using Avenga.NotesApp.Mappers;
using Avenga.NotesApp.Services.Interfaces;
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
        private readonly IRepository<User> _userRepository;

        public NoteService(IRepository<Note> noteRepository, IRepository<User> userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public void AddNote(AddNoteDto addNoteDto)
        {
            //1. validation
            //User userDb = _userRepository.GetById(addNoteDto.UserId);
            //if (userDb == null) 
            //{
            //    throw new Exception();
            //}
        }

        public void DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        public List<NoteDto> GetAllNotes()
        {
            List<Note> noteDb = _noteRepository.GetAll();
            return noteDb.Select(x=>x.ToNoteDto()).ToList();
        }

        public NoteDto GetByIdNote(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            throw new NotImplementedException();
        }
    }
}
