using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public int CustomerId { get; set; }

    public string TaxCode { get; set; } = null!;

    public string BankNumber { get; set; } = null!;

    public bool Status { get; set; }

    public string? RequestName { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();

    public virtual Task? Task { get; set; }
}
