using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Option
{
    public string OptionId { get; set; } = null!;

    public int ProductId { get; set; }

    public string? OptionName { get; set; }

    public decimal? Price { get; set; }

    public virtual Product Product { get; set; } = null!;
}
