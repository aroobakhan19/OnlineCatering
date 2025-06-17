
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OnlineCatering.Models;

namespace OnlineCatering.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly OnlineCateringContext db;

        public InvoiceController(OnlineCateringContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int bookingId)
        {
            var booking = await db.Bookings
                .Include(b => b.BookingMenuItems)
                    .ThenInclude(bmi => bmi.MenuItemNoNavigation)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(Invoice invoice, List<InvoiceItem> items)
        {
            invoice.InvoiceDate = DateTime.Now;
            invoice.TotalAmount = items.Sum(i => i.Total)
                       - (invoice.Discount ?? 0)
                       + (invoice.Tax ?? 0)
                       + (invoice.AdditionalCharges ?? 0);
            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            foreach (var item in items)
            {
                item.InvoiceId = invoice.InvoiceId;
                db.InvoiceItems.Add(item);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Bookings", "Caterer");
        }

        public async Task<IActionResult> Invoices()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null)
                return RedirectToAction("Login", "Login");

            var invoices = await db.Invoices
                .Where(i => i.CatererId == catererId)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Customer)
                .ToListAsync();

            return View(invoices);
        }

        public async Task<IActionResult> DetailsPartial(int id)
        {
            var invoice = await db.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return NotFound();

            return PartialView("_InvoiceDetailsPartial", invoice);
        }

       
    }
}
