using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Token
{
    public int TokeId { get; set; }

    public string? Token1 { get; set; }

    public int UserId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public virtual User User { get; set; } = null!;
}
