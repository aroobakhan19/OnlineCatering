using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class SupplierOrder
{
    public int SuppOrderNo { get; set; }

    public DateOnly OrderDate { get; set; }

    public int? SupplierId { get; set; }

    public decimal EstimatedAmount { get; set; }

    public bool InvoiceDone { get; set; }

    public int? CatererId { get; set; }

    public virtual CatererLogin? Caterer { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual SupplierOrderChild? SupplierOrderChild { get; set; }
}
