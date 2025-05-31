using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class SupplierOrderChild
{
    public int SuppOrderNo { get; set; }

    public int IngredientNo { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? RatePerKg { get; set; }

    public virtual RawMaterial IngredientNoNavigation { get; set; } = null!;

    public virtual SupplierOrder SuppOrderNoNavigation { get; set; } = null!;
}
