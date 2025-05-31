using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineCatering.Models;

public partial class SupplierOrder
{
    public int SuppOrderNo { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public decimal EstimatedAmount { get; set; }
    [Required]
    public bool InvoiceDone { get; set; }
    [Required]
    public int CatererId { get; set; }

    public virtual CatererLogin Caterer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual ICollection<SupplierOrderChild> SupplierOrderChildren { get; set; } = new List<SupplierOrderChild>();
}
