using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public long Phone { get; set; }

    public long? Mobile { get; set; }

    public int? CatererId { get; set; }

    public virtual ICollection<SupplierOrder> SupplierOrders { get; set; } = new List<SupplierOrder>();
}
