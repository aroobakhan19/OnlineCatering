using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class BookingMenuItem
{
    public int BookingItemId { get; set; }

    public int BookingId { get; set; }

    public int MenuItemNo { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Menu MenuItemNoNavigation { get; set; } = null!;
}
