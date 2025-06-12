using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class RawMaterial
{
    public int IngredientNo { get; set; }

    public string Name { get; set; } = null!;

    public bool Stocked { get; set; }

    public int? CatererId { get; set; }

    public virtual CatererLogin? Caterer { get; set; }
}
