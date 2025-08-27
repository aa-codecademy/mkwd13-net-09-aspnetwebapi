using Avenga.NotesApp.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;

namespace Avenga.NotesApp.DataAccess.DapperRepository
{
    public class NoteDapperRepository : IRepository<Note>
    {
        private string _connectionString;
        public NoteDapperRepository(string connectionString)
        {
            _connectionString = connectionString; //DI
        }
        public void Add(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString)) 
            {
                sqlConnection.Open();

                string insertQuery = "INSERT INTO dbo.Notes(Text, Priority, Tag, UserId) VALUS(@text, @priority, @tag, @userId)";

                sqlConnection.Query(insertQuery, new
                {
                    text = entity.Text,
                    priority = entity.Priority,
                    tag = entity.Tag,
                    userId = entity.UserId
                });
            }
        }

        public void Delete(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string deleteQuery = "DELETE FROM dbo.Notes WHERE Id=@id";
                sqlConnection.Execute(deleteQuery, new {id = entity.Id});
            }
        }

        public List<Note> GetAll()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                List<Note> notesDb = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes").ToList();
                return notesDb;
            }
        }

        public Note GetById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                Note note = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes WHERE Id=@NoteId", new { NoteId = id}).FirstOrDefault();

                return note;
            }
        }

        public void Update(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string updateQuery = "UPDATE dbo.Notes SET Text = @text, Tag = @tag, Priority = @priority, UserId = @userId WHERE Id=@id";

                sqlConnection.Query(updateQuery, new
                {
                    text = entity.Text,
                    priority = entity.Priority,
                    tag = entity.Tag,
                    userId = entity.UserId,
                    id = entity.Id
                });
            }
        }
    }
}
