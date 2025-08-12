using Avenga.NotesAndTagsApp.Models;
using Avenga.NotesAndTagsApp.Models.Enums;

namespace Avenga.NotesAndTagsApp
{
    public static class StaticDb
    {
        public static List<Note> Notes = new List<Note>()
        {
            new Note(){ Text = "Do Homework", Priority = Priority.High, Tags = new List<Tag>()
                {
                    new Tag(){Name = "Homework", Color = "Blue"},
                    new Tag(){Name = "Avenga", Color = "Red"}
                }
            },
            new Note(){ Text = "Drink more Water", Priority = Priority.Medium, Tags = new List<Tag>()
                {
                    new Tag(){Name = "Healthy", Color = "Orange"},
                    new Tag(){Name = "Priority High", Color = "Red"}
                }
            },
            new Note(){ Text = "Go to the gym", Priority = Priority.Low, Tags = new List<Tag>()
                {
                    new Tag(){Name = "Exercise", Color = "Yellow"},
                    new Tag(){Name = "Priority Medium", Color = "Green"}
                }
            }
        };
    }
}
