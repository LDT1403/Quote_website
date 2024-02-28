using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int StaffId { get; set; }

    public int RequestId { get; set; }

    public string TaskName { get; set; } = null!;

    public bool Status { get; set; }

    public string Location { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Request Request { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
