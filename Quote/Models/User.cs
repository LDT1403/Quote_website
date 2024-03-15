﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Quote.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string Role { get; set; }

    public string Status { get; set; }

    public string Image { get; set; }

    public string Dob { get; set; }

    public int? ManagerId { get; set; }

    public string Position { get; set; }

    public string Email { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<User> InverseManager { get; set; } = new List<User>();

    public virtual User Manager { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}