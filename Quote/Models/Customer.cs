using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public byte[]? Image { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User User { get; set; } = null!;
}
