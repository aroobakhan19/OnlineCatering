using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int CatererId { get; set; }

    public DateOnly BookingDate { get; set; }

    public string Venue { get; set; } = null!;

    public decimal? BillAmount { get; set; }

    public string? PaymentMode { get; set; }

    public string BookingStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BookingMenuItem> BookingMenuItems { get; set; } = new List<BookingMenuItem>();

    public virtual CatererLogin Caterer { get; set; } = null!;

    public virtual Login Customer { get; set; } = null!;
}
