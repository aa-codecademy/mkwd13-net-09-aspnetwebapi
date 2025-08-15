using NotesAndTagsApp.Models;
using NotesAndTagsApp.Models.Enims;

namespace NotesAndTagsApp
{
    public static class StaticDb
    {
        public static List<User> Users = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Dino",
                LastName = "Nikolovski",
                Username = "Dnikolovski",
                Password = "Password"
            },
            new User
            {
                Id = 2,
                FirstName = "Todor",
                LastName = "Pelivanov",
                Username = "Tpelivanov",
                Password = "123"
            }
        };

        public static List<Tag> Tags = new List<Tag>
        {
            new Tag() { Id = 1, Name = "Homework", Color = "cyan" },
            new Tag() { Id = 2, Name = "Avenga", Color = "blue" },
            new Tag() { Id = 3, Name = "Healty", Color = "orange" },
            new Tag() { Id = 4, Name = "water", Color = "blue" },
            new Tag() { Id = 5, Name = "exercise", Color = "blue" },
            new Tag() { Id = 6, Name = "fit", Color = "yellow" }
        };

        public static List<Note> Notes = new List<Note>()
        {
            new Note() { Id = 1, Text = "Do homework",
                Priority = PriorityEnum.Low,
                Tags = new List<Tag>()
                {
                    new Tag() {Id = 1, Name = "Homework", Color = "cyan"},
                    new Tag() {Id = 2, Name = "Avenga", Color = "blue"}
                },
                User = Users.First(),
                UserId = Users.First().Id,
            },

            new Note() { Id = 2, Text = "Drink more water",
                Priority = PriorityEnum.High,
                Tags = new List<Tag>()
                {
                    new Tag() {Id = 3, Name = "Healthy", Color = "orange"},
                    new Tag() {Id = 4, Name = "Water", Color = "blue"}
                },
                User = Users.First(),
                UserId = Users.First().Id,
            },

            new Note() { Id = 3, Text = "Go to the gym",
                Priority = PriorityEnum.Medium,
                Tags = new List<Tag>()
                {
                    new Tag() {Id = 5, Name = "exercise", Color = "blue"},
                    new Tag() {Id = 6, Name = "Fit", Color = "yellow"}
                },
                User = Users.Last(),
                UserId = Users.Last().Id,
            }
        };
    }
}
