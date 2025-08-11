using Avenga.NotesAndTagsApp.Models;
using Avenga.NotesAndTagsApp.Models.Enums;

namespace Avenga.NotesAndTagsApp
{
    public static class StaticDb
    {
        public static int NoteId = 3;
        public static List<User> Users = new List<User>()
        { 
            new User()
            {
                Id = 1,
                FirstName = "Filip",
                LastName = "Filiposki",
                Username = "ffiliposki",
                Password = "1234567890"
            },
            new User()
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Bobski",
                Username = "bboski",
                Password = "0987654321"
            }
        };

        public static List<Tag> Tags = new List<Tag>()
        {
            new Tag() { Id = 1, Name="Homework", Color = "cyan"},
            new Tag() { Id = 2, Name="Avenga", Color = "red"},
            new Tag() { Id = 3, Name="Healthy", Color = "green"},
            new Tag() { Id = 4, Name="Water", Color = "blue"},
            new Tag() { Id = 5, Name="Exercise", Color = "grey"},
            new Tag() { Id = 6, Name="Fit", Color = "yellow"}
        };


        public static List<Note> Notes = new List<Note>()
        {
            new Note() 
            { 
                Id = 1, 
                Text = "Do Homework",
                Priority = PriorityEnum.Low,
                Tags = new List<Tag>()
                {
                    new Tag() { Id = 1, Name="Homework", Color = "cyan"},
                    new Tag() { Id = 2, Name="Avenga", Color = "red"}
                },
                User = Users.First(),
                UserId = Users.First().Id
            },
            new Note()
            {
                Id = 2,
                Text = "Drink more water",
                Priority = PriorityEnum.Medium,
                Tags = new List<Tag>()
                {
                    new Tag() { Id = 3, Name="Healthy", Color = "green"},
                    new Tag() { Id = 4, Name="Water", Color = "blue"},
                },
                User = Users.First(),
                UserId = Users.First().Id
            },
            new Note()
            {
                Id = 3,
                Text = "Go to the gym",
                Priority = PriorityEnum.High,
                Tags = new List<Tag>()
                {
                    new Tag() { Id = 5, Name="Exercise", Color = "grey"},
                    new Tag() { Id = 6, Name="Fit", Color = "yellow"}
                },
                User = Users.Last(),
                UserId = Users.Last().Id
            }
        };
    }
}
