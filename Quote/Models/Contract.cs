using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int RequestId { get; set; }

    public decimal? FinalPrice { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Request Request { get; set; } = null!;
}
