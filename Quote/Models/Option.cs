﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class Option
{
    public int OptionId { get; set; }

    public int? ProductId { get; set; }

    public string OptionName { get; set; }

    public string Price { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Product { get; set; }
}