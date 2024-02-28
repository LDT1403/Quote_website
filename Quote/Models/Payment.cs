using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ContractId { get; set; }

    public decimal? PricePay { get; set; }

    public DateTime? DatePay { get; set; }

    public bool Status { get; set; }

    public virtual Contract Contract { get; set; } = null!;
}
