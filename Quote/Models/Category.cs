﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CateName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}