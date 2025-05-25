using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Caterer
{
    public int CatererId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public int Phone { get; set; }

    public int Mobile { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
