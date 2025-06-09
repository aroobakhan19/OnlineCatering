using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Menu
{
    public int MenuItemNo { get; set; }

    public string? ItemName { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? CategoryId { get; set; }

    public int? CatererLoginId { get; set; }

    public virtual ICollection<BookingMenuItem> BookingMenuItems { get; set; } = new List<BookingMenuItem>();

    public virtual MenuCategory? Category { get; set; }

    public virtual CatererLogin? CatererLogin { get; set; }
}
