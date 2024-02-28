using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string Type { get; set; } = null!;

    public string Cccd { get; set; } = null!;

    public bool Status { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
