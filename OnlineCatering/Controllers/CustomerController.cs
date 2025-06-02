using Microsoft.AspNetCore.Mvc;
using OnlineCatering.Models;
using OnlineCatering.Models.ViewModels;
using System.Linq;

namespace OnlineCatering.Controllers
{
    public class CustomerController : Controller
    {
        OnlineCateringContext db = new OnlineCateringContext();

        [HttpGet]
        public IActionResult Caterers()
        {
            var caterers = db.CatererLogins
                .Select(c => new CatererViewModel
                {
                    Id = c.Id,
                    Name = c.Restaurant,
                    Email = c.Email
                })
                .ToList();

            return View(caterers); // Pass as strongly-typed list
        }

        [HttpGet]
        public IActionResult StartChatWithCaterer(int catererId)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("StartChatWithCaterer", new { catererId });
                return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("ChatCustomer", "Message", new { catererId });
        }
    }
}
