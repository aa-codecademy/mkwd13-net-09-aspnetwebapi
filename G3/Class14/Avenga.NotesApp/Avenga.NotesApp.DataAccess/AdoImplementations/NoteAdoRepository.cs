using Avenga.NotesApp.Domain.Enums;
using Avenga.NotesApp.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.DataAccess.AdoImplementations
{
    public class NoteAdoRepository : IRepository<Note>
    {
        private string _connectionString;
        public NoteAdoRepository(string connectionString)
        {
            _connectionString = connectionString; //DI
        }
        public void Add(Note entity)
        {
            //1. Create new connection to the SQL db
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //2.Open the connection
            sqlConnection.Open();
            //3.Create sql command
            SqlCommand command = new SqlCommand();
            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "INSERT INTO dbo.Notes(Text, Priority, Tag, UserId)" +
                                  "VALUS(@text, @priority, @tag, @userId)";
                                    //BAD EXAMPLE
                                    //$"VALUS({entity.Text}, {entity.Priority}, {entity.Tag}, {entity.UserId})";

            command.Parameters.AddWithValue("@text", entity.Text);
            command.Parameters.AddWithValue("@priority", entity.Priority);
            command.Parameters.AddWithValue("@tag", entity.Tag);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            command.ExecuteNonQuery();

            //close the connection
            sqlConnection.Close();
        }

        public void Delete(Note entity)
        {
            //1. Create new connection to the SQL db
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //2.Open the connection
            sqlConnection.Open();
            //3.Create sql command
            SqlCommand command = new SqlCommand();
            //4. Connect the command
            command.Connection = sqlConnection;

            command.CommandText = "DELETE FROM dbo.Notes WHERE Id=@id";
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();

            //close the connection
            sqlConnection.Close();
        }

        public List<Note> GetAll()
        {
            //1. Create new connection to the SQL db
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //2.Open the connection
            sqlConnection.Open();
            //3.Create sql command
            SqlCommand command = new SqlCommand();
            //4. Connect the command
            command.Connection = sqlConnection;
            //5. write the command
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
                    //User = new User() {
                    //    FirstName = (string)sqlDataReader["FristName"]
                    //}
                });
            }

            //6. CLOSE THE CONNECTION!!!
            sqlConnection.Close();

            return notesDb;
        }

        public Note GetById(int id)
        {
            //1. Create new connection to the SQL db
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //2.Open the connection
            sqlConnection.Open();
            //3.Create sql command
            SqlCommand command = new SqlCommand();
            //4. Connect the command
            command.Connection = sqlConnection;
            //5. write the command
            command.CommandText = "SELECT * FROM dbo.Notes WHERE Id=@id";
            command.Parameters.AddWithValue("@id", id);

            List<Note> notesDb = new List<Note>();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                notesDb.Add(new Note()
                {
                    Id=(int)sqlDataReader["Id"],
                    Text = (string)sqlDataReader["Text"],
                    Priority = (Priority)sqlDataReader["Priority"],
                    Tag = (Tag)sqlDataReader["Tag"],
                    UserId = (int)sqlDataReader["UserId"]
                });
            }

            //6. CLOSE THE CONNECTION!
            sqlConnection.Close();

            return notesDb.SingleOrDefault();
        }

        public void Update(Note entity)
        {
            //1. Create new connection to the SQL db
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            //2.Open the connection
            sqlConnection.Open();
            //3.Create sql command
            SqlCommand command = new SqlCommand();
            //4. Connect the command
            command.Connection = sqlConnection;
            //5. write the command
            command.CommandText = "UPDATE dbo.Notes SET Text = @text, Tag = @tag, Priority = @priority, UserId = @userId" + "WHERE Id = @id";

            command.Parameters.AddWithValue("@text", entity.Text);
            command.Parameters.AddWithValue("@tag", entity.Tag);
            command.Parameters.AddWithValue("@priority", entity.Priority);
            command.Parameters.AddWithValue("@userId", entity.UserId);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();

            sqlConnection.Close();
        }
    }
}
