using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public byte[]? Image1 { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
