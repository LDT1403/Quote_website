using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public byte[]? Image { get; set; }

    public virtual User User { get; set; } = null!;
}
