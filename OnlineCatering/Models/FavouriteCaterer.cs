using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class FavouriteCaterer
{
    public int FavouriteId { get; set; }

    public int CustomerId { get; set; }

    public int CatererId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CatererLogin Caterer { get; set; } = null!;

    public virtual Login Customer { get; set; } = null!;
}
