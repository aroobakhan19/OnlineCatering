using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? FromCustomerId { get; set; }

    public int? FromCatererId { get; set; }

    public int? ToCustomerId { get; set; }

    public int? ToCatererId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public virtual CatererLogin? FromCaterer { get; set; }

    public virtual Login? FromCustomer { get; set; }

    public virtual CatererLogin? ToCaterer { get; set; }

    public virtual Login? ToCustomer { get; set; }
}
