﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Case.Server.Data.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; }

    public DateTime Time { get; set; }

    public virtual Order Order { get; set; }
}