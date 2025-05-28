using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public int Phone { get; set; }

    public int? Mobile { get; set; }

    public int? CatererId { get; set; }

    public virtual CatererLogin? Caterer { get; set; }
}
