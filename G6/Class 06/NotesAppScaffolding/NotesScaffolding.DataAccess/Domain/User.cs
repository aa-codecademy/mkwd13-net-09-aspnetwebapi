using System;
using System.Collections.Generic;

namespace NotesScaffolding.DataAccess.Domain;

public partial class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public int? Age { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
