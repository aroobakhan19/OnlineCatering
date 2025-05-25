using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class WorkerSalary
{
    public int SalaryId { get; set; }

    public int? WorkerId { get; set; }

    public int? WorkingDays { get; set; }

    public decimal? TotalPay { get; set; }

    public string? PayMonth { get; set; }

    public string? PayYear { get; set; }

    public virtual Worker? Worker { get; set; }
}
