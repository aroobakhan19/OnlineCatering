using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class MenuCategory
{
    public int CategoryId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
