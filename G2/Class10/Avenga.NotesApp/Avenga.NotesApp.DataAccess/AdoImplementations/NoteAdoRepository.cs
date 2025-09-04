using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Avenga.NotesApp.DataAccess.AdoImplementations
{
    public class NoteAdoRepository : IRepository<Note>
    {
        private string _connectionString;

        public NoteAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            //1. Create new connection to the SQL DATABASE
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //2. Open the connection
            sqlConnection.Open();

            //3. Create a sql command
            SqlCommand command = new SqlCommand();

            //4. Connect the command
            command.Connection = sqlConnection;

            //5.SQL Query
            command.CommandText = "INSERT INTO dbo.Notes(Text, Priority, Tag, UserId)" +
                "VALUES(@text, @priority, @tag, @userId)";

            //6. Adding values to the parameters
            command.Parameters.AddWithValue("@text", entity.Text);
            command.Parameters.AddWithValue("@priority", entity.Priority);
            command.Parameters.AddWithValue("@tag", entity.Tag);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            //7. Executing the command
            command.ExecuteNonQuery();

            //8. Closing the connection
            sqlConnection.Close();
        }

        public void Delete(Note entity)
        {
            //1. Create new connection to the SQL DATABASE
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //2. Open the connection
            sqlConnection.Open();

            //3. Create a sql command
            SqlCommand command = new SqlCommand();

            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "DELETE FROM dbo.Notes WHERE Id = @id";
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
            sqlConnection.Close();

        }

        public List<Note> GetAll()
        {
            //1. Create new connection to the SQL DATABASE
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //2. Open the connection
            sqlConnection.Open();

            //3. Create a sql command
            SqlCommand command = new SqlCommand();

            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "SELECT * FROM dbo.Notes";

            List<Note> notesDb = new List<Note>();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read()) 
            {
                notesDb.Add(new Note()
                {
                    Id = (int)sqlDataReader["Id"],
                    Text = (string)sqlDataReader["Text"],
                    Priority = (Priority)sqlDataReader["Priority"],
                    Tag = (Tag)sqlDataReader["Tag"],
                    UserId = (int)sqlDataReader["UserId"]
                });
            }

            sqlConnection.Close();

            return notesDb;
        }

        public Note GetById(int id)
        {
            //1. Create new connection to the SQL DATABASE
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //2. Open the connection
            sqlConnection.Open();

            //3. Create a sql command
            SqlCommand command = new SqlCommand();

            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "SELECT * FROM dbo.Notes WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            List<Note> notesDb = new List<Note>();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                notesDb.Add(new Note()
                {
                    Id = (int)sqlDataReader["Id"],
                    Text = (string)sqlDataReader["Text"],
                    Priority = (Priority)sqlDataReader["Priority"],
                    Tag = (Tag)sqlDataReader["Tag"],
                    UserId = (int)sqlDataReader["UserId"]
                });
            }

            sqlConnection.Close();
            return notesDb.FirstOrDefault();
        }

        public void Update(Note entity)
        {
            //1. Create new connection to the SQL DATABASE
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //2. Open the connection
            sqlConnection.Open();

            //3. Create a sql command
            SqlCommand command = new SqlCommand();

            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "UPDATE dbo.Notes SET Text = @text, Tag = @tag, Priority = @priority, UserId = @userId" +
                " WHERE Id = @id";

            command.Parameters.AddWithValue("@text", entity.Text);
            command.Parameters.AddWithValue("@priority", entity.Priority);
            command.Parameters.AddWithValue("@tag", entity.Tag);
            command.Parameters.AddWithValue("@userId", entity.UserId);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
