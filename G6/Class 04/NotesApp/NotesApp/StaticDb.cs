using NotesApp.Models;
using NotesApp.Models.Enums;

namespace NotesApp
{
	public static class StaticDb
	{
		public static List<User> Users = new List<User>()
		{
			new User
			{
				Id = 1,
				FirstName = "Petko",
				LastName = "Petkovski",
				Username = "p.petko",
				Password = "P@ssw0rd"
			},
			new User
			{
				Id = 2,
				FirstName = "Marko",
				LastName = "Markovski",
				Username = "m.marko",
				Password = "Test123"
			}
		};

		public static List<Tag> Tags = new List<Tag>()
		{
			new Tag() {Id = 1, Name = "Homework", Color = "blue"},
			new Tag() {Id = 2, Name = "Avenga Academy", Color = "purple"},
			new Tag() {Id = 3, Name = "Health", Color = "green"},
			new Tag() {Id = 4, Name = "Exercise", Color = "red"},
			new Tag() {Id = 5, Name = "Fit", Color = "yellow"},
		};

		public static List<Note> Notes = new List<Note>()
		{
			new Note()
			{
				Id = 1,
				Text = "Do the homework",
				Priority = PriorityEnum.Medium,
				Tags = new List<Tag>()
				{
					Tags[0], Tags[1]
				},
				User = Users.First(),
				UserId = Users.First().Id
			},
			new Note()
			{
				Id = 2,
				Text = "Drink more water",
				Priority = PriorityEnum.High,
				Tags = new List<Tag>()
				{
					Tags[2]
				},
				User = Users[1],
				UserId = Users[1].Id
			},
			new Note()
			{
				Id = 3,
				Text = "Go to the gym",
				Priority = PriorityEnum.Low,
				Tags = new List<Tag>()
				{
					Tags[2], Tags[3]
				},
				User = Users.Last(),
				UserId = Users.Last().Id
			},
		};
	}
}
