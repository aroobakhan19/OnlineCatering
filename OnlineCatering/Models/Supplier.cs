using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Pincode { get; set; }

    public int? Phone { get; set; }

    public int? Mobile { get; set; }
}
