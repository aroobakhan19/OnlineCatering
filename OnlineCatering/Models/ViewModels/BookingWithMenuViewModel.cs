using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCatering.Models;

namespace OnlineCatering.Models.ViewModels
{
    public class BookingWithMenuViewModel
    {
        public int CatererId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly BookingDate { get; set; }

        [Required]
        public string Venue { get; set; } = string.Empty;

        public string? PaymentMode { get; set; }
        public List<SelectListItem> PaymentModes { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Text = "Cash", Value = "Cash" },
        new SelectListItem { Text = "Online Transfer", Value = "Online Transfer" }
    };

        // All menu items of the caterer to show in checkbox list
        public List<Menu> MenuItems { get; set; } = new List<Menu>();

        // IDs of selected menu items
        public List<int> SelectedMenuItemNos { get; set; } = new List<int>();
    }
}
