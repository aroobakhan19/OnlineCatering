using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int BookingId { get; set; }

    public int CatererId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Tax { get; set; }

    public decimal? AdditionalCharges { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string? InvoiceFilePath { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual CatererLogin Caterer { get; set; } = null!;

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
