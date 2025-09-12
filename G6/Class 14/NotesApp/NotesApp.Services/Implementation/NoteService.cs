using Dapper;
using Microsoft.Data.SqlClient;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Mappers;
using NotesApp.Services.Interfaces;
using System.Data;
using System.Security.Cryptography;

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

        public void AddNote(AddNoteDto note, int userId)
        {
            if (note == null) throw new ArgumentNullException("Model should populated!");

            if (string.IsNullOrEmpty(note.Text)) throw new ArgumentNullException("Text is required");

            if (note.Text.Length > 100) throw new ArgumentException("Text lenght should be max 100 chars");

            //if (note.Tag == null) throw new ArgumentNullException("Tag is required"); //Tag is not nullable, so this condition will be false always

            var user = _userRepository.GetById(userId);

            if (user == null) throw new ArgumentException($"User with id: {userId}, is not found");

            var noteDb = new Note
            {
                Text = note.Text,
                Priority = note.Priority,
                Tag = note.Tag,
                UserId = userId
            };

            _noteRepository.Add(noteDb);
        }

        //EF
        public List<NoteDto> GetAllNotes(FilterDto filter, int userId)
        {
            //var notes = _noteRepository.GetAll(filter.SearchText ?? string.Empty);
            //var notes = _noteRepository.GetAll(string.IsNullOrEmpty(filter.SearchText) ? string.Empty : filter.SearchText);

            var notes = _noteRepository.GetAll().Where(x => x.UserId == userId && (string.IsNullOrEmpty(filter.SearchText) || x.Text.Contains(filter.SearchText)));

            var notesDto = notes.Select(x => x.ToDto()).ToList();

            return notesDto;
        }

        //ADO.NET
        //public List<NoteDto> GetAllNotes(FilterDto filter)
        //{
        //    SqlConnection connection = new SqlConnection("Server=.\\SQLExpress;Database=AANotesApp;Trusted_Connection=True;TrustServerCertificate=True");
        //    connection.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = connection;
        //    //SqlCommand cmd = connection.CreateCommand();
        //    //SQL injection attack
        //    //cmd.CommandText = @"SELECT *
        //    //                    FROM Notes n
        //    //                    INNER JOIN Users u ON n.UserId = u.Id
        //    //                    WHERE n.[Text] LIKE '%" + filter.SearchText + "%'";

        //    cmd.CommandText = @"SELECT *
        //                        FROM Notes n
        //                        INNER JOIN Users u ON n.UserId = u.Id
        //                        WHERE n.[Text] LIKE @SearchText";

        //    cmd.Parameters.AddWithValue("@SearchText", $"%{filter.SearchText}%");

        //    SqlDataReader dr = cmd.ExecuteReader();

        //    var result = new List<NoteDto>();

        //    while (dr.Read())
        //    {
        //        var note = new NoteDto
        //        {
        //            Id = (int)dr["Id"],
        //            Priority = (PriorityEnum)dr["Priority"],
        //            Tag = (TagEnum)dr["Tag"],
        //            Text = (string)dr["Text"],
        //            UserFullName = $"{(string)dr["FirstName"]} {(string)dr["LastName"]}"
        //        };

        //        result.Add(note);
        //    }

        //    connection.Close();

        //    return result;
        //}

        //Dapper
        //public List<NoteDto> GetAllNotes(FilterDto filter)
        //{
        //    IDbConnection connection = new SqlConnection("Server=.\\SQLExpress;Database=AANotesApp;Trusted_Connection=True;TrustServerCertificate=True");
        //    connection.Open();

        //    //SQL injection attack
        //    //var sqlQuery = @$"SELECT n.Id,
        //    //                n.Text,
        //    //                n.Priority,
        //    //                n.Tag,
        //    //                u.FirstName + ' ' + u.LastName AS UserFullName
        //    //           FROM Notes n
        //    //           INNER JOIN Users u ON n.UserId = u.Id
        //    //           WHERE n.[Text] LIKE '%{filter.SearchText}%'";

        //    var sqlQuery = @$"SELECT n.Id,
        //                    n.Text,
        //                    n.Priority,
        //                    n.Tag,
        //                    u.FirstName + ' ' + u.LastName AS UserFullName
        //               FROM Notes n
        //               INNER JOIN Users u ON n.UserId = u.Id
        //               WHERE n.[Text] LIKE @SearchText";

        //    var parametars = new { @SearchText = $"%{filter.SearchText}%" };

        //    var result = connection.Query<NoteDto>(sqlQuery, parametars).ToList();

        //    connection.Close();

        //    return result;
        //}

        public NoteDto GetById(int id, int userId)
        {
            var note = _noteRepository.GetById(id);

            if (note.UserId != userId)
            {
                throw new Exception("The logged in user is not owner of the requrest note");
            }

            return note.ToDto();
        }

        public void DeleteById(int id)
        {
            var note = _noteRepository.GetById(id);
            _noteRepository.Delete(note);
        }

        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            //validation
            if(updateNoteDto == null)
            {
				throw new ArgumentNullException("Model should populated!");
			}

            Note noteDb = _noteRepository.GetById(updateNoteDto.Id);
            if(noteDb == null)
            {
                throw new Exception($"Note with id {updateNoteDto.Id} was not found");
            }

			var isValidUser = noteDb.UserId == updateNoteDto.UserId;
			if (!isValidUser)
			{

				throw new Exception($"This note is not a note of the user with id {updateNoteDto.UserId}");
			}

			if (string.IsNullOrEmpty(updateNoteDto.Text)) throw new ArgumentNullException("Text is required");

			if (updateNoteDto.Text.Length > 100) throw new ArgumentException("Text lenght should be max 100 chars");

            User userDb = _userRepository.GetById(updateNoteDto.UserId);
            if(userDb == null)
            {
                throw new DataException($"User with id {updateNoteDto.UserId} was not found");
            }

            //update
            noteDb.Text = updateNoteDto.Text;
            noteDb.Priority = updateNoteDto.Priority;
            noteDb.Tag = updateNoteDto.Tag;
            noteDb.User = userDb;
            noteDb.UserId = updateNoteDto.UserId;

            //save to db (call db)
            _noteRepository.Update(noteDb);
		}
    }
}
