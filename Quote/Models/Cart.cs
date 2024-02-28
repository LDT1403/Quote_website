using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Customer Customer { get; set; } = null!;
}
