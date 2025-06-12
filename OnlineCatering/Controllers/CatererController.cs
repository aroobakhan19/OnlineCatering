using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCatering.Models;
using OnlineCatering.Models.ViewModels;
namespace OnlineCatering.Controllers
{
    public class CatererController : Controller
    {
        OnlineCateringContext db = new OnlineCateringContext();
        public IActionResult Index()
        {
            return View();
        }

        //Controller of Menu categories
        
        [HttpPost]
        public IActionResult addMenuCategory(MenuCategory menuCategory)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            if (ModelState.IsValid)
            {
                menuCategory.CatererLoginId = catererId.Value;
                db.MenuCategories.Add(menuCategory);
                db.SaveChanges();
            }
            return RedirectToAction("Menu_Categories");
        }

        [HttpGet]
        public IActionResult Menu_Categories()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            Console.WriteLine("CatererID",catererId.Value);
            return View(db.MenuCategories.ToList());
        }

       
        [HttpPost]
        [HttpPost]
        public IActionResult Edit_MenuCategory(int id, MenuCategory menuCategory)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }

            if (ModelState.IsValid)
            {
                // Ensure the ID from the route is used
                menuCategory.CategoryId = id;
                db.Update(menuCategory);
                db.SaveChanges();
            }

            return RedirectToAction("Menu_Categories");
        }

        public IActionResult Detail_MenuCategory(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            var t = (from data in db.MenuCategories where data.CategoryId == id select data).FirstOrDefault();

            return View(t);
        }

       
        [HttpPost, ActionName("delete_MenuCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteConfirmed(int id) {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            var category =  db.MenuCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            var menus = db.Menus.Where(m => m.CategoryId == id).ToList();
            db.Menus.RemoveRange(menus); // delete menus first

            db.MenuCategories.Remove(category); // now delete the category
            db.SaveChanges();

            return RedirectToAction("Menu_Categories");
        }

        //Controller of Menu categories


        [HttpPost]
        public IActionResult addMenu(Menu menu, IFormFile file)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            if (file != null && file.Length > 0)
            {
                var imageName = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");

                // Ensure the directory exists
                if (!Directory.Exists(imagePath))
                    Directory.CreateDirectory(imagePath);

                string imagevalue = Path.Combine(imagePath, imageName);

                using (var stream = new FileStream(imagevalue, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                menu.Image = Path.Combine("/Image/", imageName);
            }
            else
            {
                // File is null — handle gracefully
                ModelState.AddModelError("Image", "Please upload an image.");
                ViewData["CategoryId"] = new SelectList(db.MenuCategories, "CategoryId", "Category");
                return View(menu);
            }
            menu.CatererLoginId = catererId.Value;
            db.Menus.Add(menu);
            db.SaveChanges();

            return RedirectToAction("listMenu");
        }



        [HttpGet]
        public IActionResult listMenu()
        {
            ViewBag.CategoryId = new SelectList(db.MenuCategories.ToList(), "CategoryId", "Category");

            var menus = db.Menus.Include(m => m.Category).ToList();
            return View(menus);

        }

        //[HttpGet]
        //public IActionResult deleteMenu(int id) { 
        //    var data = db.Menus.Find(id);
        //    return View(data);
        //}

        [HttpPost, ActionName("deleteMenuConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteMenuConfirmed(int id) 
        {
            var Menu = db.Menus.Find(id);
            if (Menu == null) return NotFound();
            db.Menus.Remove(Menu);
            db.SaveChanges();
            return RedirectToAction("listMenu");
        }

        //[HttpGet]
        //public IActionResult editMenu(int id) 
        //{
        //    var menu = db.Menus.Find(id);
        //    if (menu == null) return NotFound();

        //    ViewData["CategoryId"] = new SelectList(db.MenuCategories, "CategoryId", "Category");
        //    return View(menu);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editMenu(Menu menu, IFormFile file, int id)
        {
            var existingMenu = db.Menus.Find(id);
            if (existingMenu == null) return NotFound();

            if (file != null && file.Length > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image");
                Directory.CreateDirectory(imagePath);
                string fullPath = Path.Combine(imagePath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                existingMenu.Image = Path.Combine("/Image/", fileName);
            }
            existingMenu.ItemName = menu.ItemName;
            existingMenu.CategoryId = menu.CategoryId;
            existingMenu.Price = menu.Price;
            existingMenu.Description = menu.Description;

            db.SaveChanges();

            return RedirectToAction("listMenu");
        }


        public IActionResult detailMenu(int id)
        {
            var menu = db.Menus
                     .Include(m => m.Category)
                     .FirstOrDefault(m => m.MenuItemNo == id);
            return View(menu);
        }

        //Controller of Menu

        //Controller of WorkerType 
        [HttpGet]
        public IActionResult WorkerType()
        {
            return View(db.WorkerTypes.ToList());
        }



        [HttpPost]
        public IActionResult AddWorkersType(WorkerType workerType)
        {
            if (ModelState.IsValid)
            {
                db.WorkerTypes.Add(workerType);
                db.SaveChanges();
            }
            return RedirectToAction("WorkerType");
        }

        
        [HttpPost]
        public IActionResult editWorkerType(int id, WorkerType workerType)
        {
            var existingWorkerType = db.WorkerTypes.Find(id);
            if (existingWorkerType == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingWorkerType.WorkerType1 = workerType.WorkerType1;
                existingWorkerType.PayPerDay = workerType.PayPerDay;
                db.SaveChanges();
            }
            return RedirectToAction("WorkerType");

        }


        [HttpGet]
        public IActionResult workerTypeDetail(int id) 
        {
            var workerType = (from data in db.WorkerTypes where data.WorkerTypeId == id select data).FirstOrDefault();
            return View(workerType);
        }

      [HttpPost, ActionName("deleteWorkerType")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteWorkerType(int id)
        {
            var workerType = db.WorkerTypes.Find(id);
            if (workerType == null) return NotFound();
            db.WorkerTypes.Remove(workerType);
            db.SaveChanges();
            return RedirectToAction("WorkerType");
        }

        //Controller of WorkerType 

        //Controller of Worker

        [HttpPost]
        public IActionResult AddWorkers(Worker workers)
        {
            ViewData["WorkerTypeId"] = new SelectList(db.WorkerTypes, "WorkerTypeId", "WorkerType1");
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }
            if (ModelState.IsValid)
            {
                workers.CatererId = catererId.Value;
                db.Workers.Add(workers);
                db.SaveChanges();
            }
            return RedirectToAction("Workers");
        }

        public IActionResult Workers()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }
            var workers = db.Workers.Include(w => w.WorkerType).ToList();
            ViewBag.WorkerTypes = db.WorkerTypes.ToList(); // ✅ FIXED LINE
            return View(workers);
        }

        [HttpPost]
        public IActionResult editWorkers(int id,  Worker worker)
        {
            ViewData["WorkerTypeId"] = new SelectList(db.WorkerTypes, "WorkerTypeId", "WorkerType1");

            var existingWorker = db.Workers.Find(id);
            if (existingWorker == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingWorker.Name = worker.Name;
                existingWorker.Address = worker.Address;
                existingWorker.Phone = worker.Phone;
                existingWorker.Mobile = worker.Mobile;
                existingWorker.DateOfJoin = worker.DateOfJoin;
                existingWorker.WorkerTypeId = worker.WorkerTypeId;
                db.SaveChanges();
            return RedirectToAction("Workers");
            }
            return View(worker);

        }

        [HttpPost, ActionName("deleteWorker")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteWorker(int id)
        {
            var worker = db.Workers.Find(id);
            if (worker == null) return NotFound();

            db.Workers.Remove(worker);
            db.SaveChanges();

            return RedirectToAction("Workers");
        }


        public IActionResult workerDetail(int id) 
        {
            var worker = db.Workers
                   .Include(w => w.WorkerType) 
                   .FirstOrDefault(w => w.WorkerId == id);


            if (worker == null) return NotFound();
            return View(worker);
        }

        //Controller of Worker



        //Controllers of Raw Material


        [HttpPost]
        public IActionResult addRawMaterials(RawMaterial rawMaterial)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Validation failed";
                return RedirectToAction("RawMaterials");
            }

            rawMaterial.CatererId = catererId.Value;
            db.RawMaterials.Add(rawMaterial);
            db.SaveChanges();

            return RedirectToAction("RawMaterials");
        }


        [HttpGet]
        public IActionResult RawMaterials()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }

            var rawMaterials = db.RawMaterials
                                 .Where(r => r.CatererId == catererId)
                                 .ToList();

            return View(rawMaterials);
        }



        [HttpPost]
        public IActionResult editRawMaterial(RawMaterial rawMaterial, int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            var existingMaterial = db.RawMaterials
                                     .FirstOrDefault(r => r.IngredientNo == id && r.CatererId == catererId);

            if (existingMaterial == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingMaterial.Name = rawMaterial.Name;
                existingMaterial.Stocked = rawMaterial.Stocked;
                db.SaveChanges();
                return RedirectToAction("RawMaterials");
            }

            return View(existingMaterial);
        }




        [HttpGet]
        public IActionResult detailRawMaterial(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            var existingMaterial = db.RawMaterials
                                     .FirstOrDefault(r => r.IngredientNo == id && r.CatererId == catererId);

            if (existingMaterial == null)
                return NotFound();

            return View(existingMaterial);
        }


        [HttpPost, ActionName("deleteRawMaterial")]
        [ValidateAntiForgeryToken]
        public IActionResult deletedRawMaterial(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            var existingMaterial = db.RawMaterials
                                     .FirstOrDefault(r => r.IngredientNo == id && r.CatererId == catererId);

            if (existingMaterial == null)
                return NotFound();

            db.RawMaterials.Remove(existingMaterial);
            db.SaveChanges();
            return RedirectToAction("RawMaterials");
        }


        //Controllers of Raw Material



        //Controller of Supplier

        [HttpGet]
        public IActionResult Suppliers()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                return RedirectToAction("CatererLogin", "Login");
            }

            var suppliers = db.Suppliers
                .Where(s => s.CatererId == catererId.Value)
                .ToList();

            return View(suppliers);
        }



        [HttpPost]
        public IActionResult addSuppliers(Supplier supplier)
        {
            if (supplier == null) return NotFound();
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }

            if (ModelState.IsValid)
            {
                supplier.CatererId = catererId.Value;
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Suppliers");
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier == null) return NotFound();

            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Suppliers");

        }

       
        [HttpGet]
        public IActionResult detailSupplier(int id)
        {
            var supplier = (from data  in db.Suppliers where data.SupplierId == id select data).FirstOrDefault();
            if (supplier == null) return NotFound();
            return View(supplier);
        }
        [HttpPost]
        public IActionResult editSupplier(Supplier supplier)
        {
            if (supplier == null) return BadRequest();

            var existingsupplier = db.Suppliers.Find(supplier.SupplierId);
            if (existingsupplier == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingsupplier.Name = supplier.Name;
                existingsupplier.Address = supplier.Address;
                existingsupplier.Pincode = supplier.Pincode;
                existingsupplier.Phone = supplier.Phone;
                existingsupplier.Mobile = supplier.Mobile;

                db.SaveChanges();
                return RedirectToAction("Suppliers");
            }
            return View(supplier);
        }



        //Controller of SupplierOrder



        public IActionResult ListSupplierOrder()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null)
                return RedirectToAction("CatererLogin", "Login");

            ViewBag.SupplierId = new SelectList(db.Suppliers.Where(s => s.CatererId == catererId), "SupplierId", "Name");

            var orders = db.SupplierOrders
                .Include(o => o.Supplier)
                .Where(o => o.CatererId == catererId.Value)
                .ToList();

            return View(orders);
        }

        // POST: Add new (from the modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSupplierOrder(SupplierOrder order, IFormFile file)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null)
                return RedirectToAction("CatererLogin", "Login");

            // repopulate dropdown
            ViewBag.SupplierId = new SelectList(
                db.Suppliers.Where(s => s.CatererId == catererId.Value),
                "SupplierId", "Name"
            );

            // NEW: if they tick “Done” but didn’t upload
            if (order.InvoiceDone && (file == null || file.Length == 0))
            {
                ModelState.AddModelError(
    "file",    
    "You must upload an invoice image if you mark this order as Done."
);
                ;
            }

            if (!ModelState.IsValid)
            {
                var orders = db.SupplierOrders
                    .Include(o => o.Supplier)
                    .Where(o => o.CatererId == catererId.Value)
                    .ToList();
                return View("ListSupplierOrder", orders);
            }

            if (file != null && file.Length > 0)
            {
                var imageName = Path.GetFileName(file.FileName);
                var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/InvoiceImages");
                if (!Directory.Exists(imageDir)) Directory.CreateDirectory(imageDir);

                var fullPath = Path.Combine(imageDir, imageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    file.CopyTo(stream);

                order.InvoicePicture = "/InvoiceImages/" + imageName;
            }

            order.CatererId = catererId.Value;
            db.SupplierOrders.Add(order);
            db.SaveChanges();

            return RedirectToAction("ListSupplierOrder");
        }


        // POST: Delete
        [HttpPost]
        public IActionResult DeleteSupplierOrder(int id)
        {
            var order = db.SupplierOrders.Find(id);
            if (order == null) return NotFound();

            db.SupplierOrders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("ListSupplierOrder");
        }

        // POST: Edit (from the modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSupplierOrder(SupplierOrder updatedOrder, IFormFile file)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null)
                return RedirectToAction("CatererLogin", "Login");

            // Load the existing record once
            var existing = db.SupplierOrders.Find(updatedOrder.SuppOrderNo);
            if (existing == null)
                return NotFound();

            // ← ADD THIS TO REMOVE the binder’s “required” error on file
            ModelState.Remove("file");

            // Only require a new file if marking done AND no existing image AND no upload
            if (updatedOrder.InvoiceDone
                && string.IsNullOrEmpty(existing.InvoicePicture)
                && (file == null || file.Length == 0))
            {
                ModelState.AddModelError("file",
                    "You must upload an invoice image if you mark this order as Done.");
            }


            // Debug print all ModelState errors
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Model error for {key}: {error.ErrorMessage}");
                }
            }

            // If there are validation errors, redirect back and reopen the modal
            if (!ModelState.IsValid)
            {
                TempData["EditError"] = "Something went wrong while editing the order.";
                TempData["OpenModalId"] = updatedOrder.SuppOrderNo;
                return RedirectToAction("ListSupplierOrder");
            }

            // Map the updated fields
            existing.OrderDate = updatedOrder.OrderDate;
            existing.SupplierId = updatedOrder.SupplierId;
            existing.EstimatedAmount = updatedOrder.EstimatedAmount;
            existing.InvoiceDone = updatedOrder.InvoiceDone;

            // If a new file was uploaded, save it and update the path
            if (file != null && file.Length > 0)
            {
                var imageName = Path.GetFileName(file.FileName);
                var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/InvoiceImages");
                if (!Directory.Exists(imageDir))
                    Directory.CreateDirectory(imageDir);

                var fullPath = Path.Combine(imageDir, imageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                existing.InvoicePicture = "/InvoiceImages/" + imageName;
            }
            // else: leave existing.InvoicePicture untouched

            db.SaveChanges();
            return RedirectToAction("ListSupplierOrder");
        }


        // GET: Caterer/Bookings
        public async Task<IActionResult> Bookings()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId"); // assuming caterer is logged in and Id stored here
            if (catererId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var bookings = await db.Bookings
                .Where(b => b.CatererId == catererId)
                .Include(b => b.Customer) // eager load customer details if needed
                .Include(b => b.BookingMenuItems)
                    .ThenInclude(bmi => bmi.MenuItemNoNavigation) // eager load menu items
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            
            var viewModel = bookings.Select(b => new BookingWithInvoiceViewModel
            {
                Booking = b,
                InvoiceExists = db.Invoices.Any(i => i.BookingId == b.BookingId)
            }).ToList();

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, string newStatus)
        {
            var booking = await db.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            booking.BookingStatus = newStatus;
            await db.SaveChangesAsync();

            return RedirectToAction("Bookings");
        }

    }
}
