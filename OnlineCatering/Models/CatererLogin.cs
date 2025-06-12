using System;
using System.Collections.Generic;

namespace OnlineCatering.Models;

public partial class CatererLogin
{
    public int Id { get; set; }

    public string Restaurant { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime? RegDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FavouriteCaterer> FavouriteCaterers { get; set; } = new List<FavouriteCaterer>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Message> MessageFromCaterers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageToCaterers { get; set; } = new List<Message>();

    public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();

    public virtual ICollection<SupplierOrder> SupplierOrders { get; set; } = new List<SupplierOrder>();
}
