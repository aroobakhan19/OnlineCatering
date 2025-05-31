using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class RawMaterial
{
    public int IngredientNo { get; set; }

    public string Name { get; set; } = null!;

    public int CatererId { get; set; }

    public virtual CatererLogin Caterer { get; set; } = null!;

    public virtual ICollection<SupplierOrderChild> SupplierOrderChildren { get; set; } = new List<SupplierOrderChild>();
}
