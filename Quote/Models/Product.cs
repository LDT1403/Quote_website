using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();
}
