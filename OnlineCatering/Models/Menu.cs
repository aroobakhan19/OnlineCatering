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

    public virtual MenuCategory? Category { get; set; }
}
