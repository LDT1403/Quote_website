using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class RequestDetail
{
    public int RequestDetailId { get; set; }

    public int RequestId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
