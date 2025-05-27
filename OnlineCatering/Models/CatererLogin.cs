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

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
