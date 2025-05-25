using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public int? WorkerTypeId { get; set; }

    public int? CatererId { get; set; }

    public DateOnly? DateOfJoin { get; set; }

    public virtual Caterer? Caterer { get; set; }

    public virtual ICollection<WorkerSalary> WorkerSalaries { get; set; } = new List<WorkerSalary>();

    public virtual WorkerType? WorkerType { get; set; }
}
