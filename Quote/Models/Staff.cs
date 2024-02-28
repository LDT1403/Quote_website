using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int UserId { get; set; }

    public int? ManagerId { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<Staff> InverseManager { get; set; } = new List<Staff>();

    public virtual Staff? Manager { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User User { get; set; } = null!;
}
