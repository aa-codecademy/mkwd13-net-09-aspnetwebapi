using Avenga.NotesApp.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Avenga.NotesApp.DataAccess.DapperImplementations
{
    public class NoteDapperRepository :IRepository<Note>
    {
        private string _connectionString;

        public NoteDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string insertQuery = "INSERT into dbo.Notes(Text, Priority, Tag, UserId) VALUES(@text, @priority, @tag, @userId)";

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

                string DeleteQuery = "DELETE FROM dbo.Notes WHERE Id = @id";
                sqlConnection.Execute(DeleteQuery, new { id = entity.Id });
            }
        }

        public List<Note> GetAll()
        {
            //BAD WAY
            //SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //sqlConnection.Open();
            //List<Note> notesDb = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes").ToList();
            //return notesDb;
            //sqlConnection.Close(); // NOT REACHABLE

            //GOOD WAY => use USING block whenever you can 
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

                Note noteDb = sqlConnection.Query<Note>("SELECT * FROM dbo.Notes WHERE Id = @NoteId", new { NoteId = id }).FirstOrDefault();
                return noteDb;
            }
        }

        public void Update(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string updateQuery = "UPDATE dbo.Notes SET Text = @text, Tag = @tag, Priority = @priority, UserId = @userId" +
                    " WHERE Id = @id";

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
