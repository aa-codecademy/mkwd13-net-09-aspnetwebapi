using System.ComponentModel.DataAnnotations;

namespace Avenga.NotesAppScaffolded.DataAccess.Domain;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [MaxLength(100)]
    public string? Address { get; set; }

    public int Age { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
