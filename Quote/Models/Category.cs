using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CateName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
