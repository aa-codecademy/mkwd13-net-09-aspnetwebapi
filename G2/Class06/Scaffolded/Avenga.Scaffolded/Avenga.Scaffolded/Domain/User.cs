using System;
using System.Collections.Generic;

namespace Avenga.Scaffolded.Domain;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
