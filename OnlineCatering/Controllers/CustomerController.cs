using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCatering.Models;
using OnlineCatering.Models.ViewModels;
using System.Linq;

namespace OnlineCatering.Controllers
{
    public class CustomerController : Controller
    {
        OnlineCateringContext db = new OnlineCateringContext();

        [HttpGet]
        [HttpGet]
        public IActionResult Caterers()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");

            var favouritedIds = new List<int>();
            if (customerId != null)
            {
                favouritedIds = db.FavouriteCaterers
                    .Where(f => f.CustomerId == customerId)
                    .Select(f => f.CatererId)
                    .ToList();
            }

            var caterers = db.CatererLogins
                .Select(c => new CatererViewModel
                {
                    Id = c.Id,
                    Name = c.Restaurant,
                    Email = c.Email,
                    IsFavourited = favouritedIds.Contains(c.Id)
                })
                .ToList();

            return View(caterers);
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


        [HttpGet]
        public IActionResult MenuByCaterer(int catererId, string search = "", string category = "", decimal? minPrice = null, decimal? maxPrice = null)
        {
            // Get filtered menus with category name filtering
            var menus = db.Menus
                .Where(m => m.CatererLoginId == catererId &&
                            (string.IsNullOrEmpty(search) || m.ItemName.Contains(search)) &&
                            (string.IsNullOrEmpty(category) || m.Category.Category == category) &&
                            (!minPrice.HasValue || m.Price >= minPrice) &&
                            (!maxPrice.HasValue || m.Price <= maxPrice))
                .Include(m => m.Category) // Include the navigation property to access CategoryName
                .ToList();

            ViewBag.CatererName = db.CatererLogins.Find(catererId)?.Restaurant;
            ViewBag.CatererId = catererId;
            ViewBag.Categories = db.MenuCategories.Select(c => c.Category).ToList();

            return View(menus);
        }


        [HttpGet]
        public IActionResult AddToFavourites(int catererId)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("AddToFavourites", new { catererId });
                return RedirectToAction("Login", "Login");
            }

            bool alreadyFavourite = db.FavouriteCaterers.Any(f =>
                f.CustomerId == customerId && f.CatererId == catererId);

            if (!alreadyFavourite)
            {
                var favourite = new FavouriteCaterer
                {
                    CustomerId = customerId.Value,
                    CatererId = catererId
                };
                db.FavouriteCaterers.Add(favourite);
                db.SaveChanges();
            }

            TempData["Success"] = "Caterer added to favourites!";
            return RedirectToAction("FavouriteCaterers");
        }

        [HttpGet]
        public IActionResult FavouriteCaterers()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("FavouriteCaterers");
                return RedirectToAction("Login", "Login");
            }

            var favourites = db.FavouriteCaterers
                .Where(f => f.CustomerId == customerId)
                .Include(f => f.Caterer)
                .Select(f => new CatererViewModel
                {
                    Id = f.Caterer.Id,
                    Name = f.Caterer.Restaurant,
                    Email = f.Caterer.Email
                })
                .ToList();

            return View(favourites);
        }

        [HttpGet]
        public IActionResult RemoveFromFavourites(int catererId)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("FavouriteCaterers");
                return RedirectToAction("Login", "Login");
            }

            var favourite = db.FavouriteCaterers.FirstOrDefault(f =>
                f.CustomerId == customerId && f.CatererId == catererId);

            if (favourite != null)
            {
                db.FavouriteCaterers.Remove(favourite);
                db.SaveChanges();
                TempData["Success"] = "Removed from favourites.";
            }

            return RedirectToAction("Caterers");
        }



        [HttpGet]
        public async Task<IActionResult> BookWithMenu(int catererId)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("FavouriteCaterers");
                return RedirectToAction("Login", "Login");
            }
            // Get caterer menu items
            var menuItems = await db.Menus
                .Where(m => m.CatererLoginId == catererId)
                .ToListAsync();

            var model = new BookingWithMenuViewModel
            {
                CatererId = catererId,
                MenuItems = menuItems
            };

            return View(model);
        }

        // POST: Customer/BookWithMenu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookWithMenu(BookingWithMenuViewModel model)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                TempData["ReturnUrl"] = Url.Action("FavouriteCaterers");
                return RedirectToAction("Login", "Login");
            }
            if (!ModelState.IsValid)
            {
                // Reload menu items for redisplay
                model.MenuItems = await db.Menus
                    .Where(m => m.CatererLoginId == model.CatererId)
                    .ToListAsync();
                return View(model);
            }
            decimal billAmount = 0;

            if (model.SelectedMenuItemNos != null && model.SelectedMenuItemNos.Any())
            {
                billAmount = await db.Menus
                    .Where(m => model.SelectedMenuItemNos.Contains(m.MenuItemNo))
                    .SumAsync(m => m.Price ?? 0); // assuming Price nullable decimal
            }

            // Create Booking entity
            var booking = new Booking
            {
                CustomerId = customerId.Value,
                CatererId = model.CatererId,
                BookingDate = model.BookingDate,
                Venue = model.Venue,
                PaymentMode = model.PaymentMode ?? "",
                BookingStatus = "Pending",
                BillAmount = billAmount,
                CreatedAt = DateTime.Now
            };

            db.Bookings.Add(booking);
            await db.SaveChangesAsync(); // Get BookingId generated

            // Add BookingMenuItems for selected menu items
            if (model.SelectedMenuItemNos != null && model.SelectedMenuItemNos.Any())
            {
                foreach (var menuItemNo in model.SelectedMenuItemNos)
                {
                    var bookingMenuItem = new BookingMenuItem
                    {
                        BookingId = booking.BookingId,
                        MenuItemNo = menuItemNo
                    };
                    db.BookingMenuItems.Add(bookingMenuItem);
                }
                await db.SaveChangesAsync();
            }

            TempData["Success"] = "Booking created successfully!";
            return RedirectToAction("Caterers", "Customer"); // or wherever you want
        }




    }
}
