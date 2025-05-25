using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class WorkerType
{
    public int WorkerTypeId { get; set; }

    public string? WorkerType1 { get; set; }

    public decimal? PayPerDay { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
