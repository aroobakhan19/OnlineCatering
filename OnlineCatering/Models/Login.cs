using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class Login
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string UserAddress { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FavouriteCaterer> FavouriteCaterers { get; set; } = new List<FavouriteCaterer>();

    public virtual ICollection<Message> MessageFromCustomers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageToCustomers { get; set; } = new List<Message>();
}
