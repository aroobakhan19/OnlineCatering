using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCatering.Models;
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
        public IActionResult addMenuCategory()
        {
            return View();
        }

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
            return View(db.MenuCategories.ToList());
        }

        [HttpGet]
        public IActionResult Edit_MenuCategory(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            var data = db.MenuCategories.Find(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit_MenuCategory(int id, MenuCategory menuCategory)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            if (ModelState.IsValid)
            {
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

        [HttpGet]
        public IActionResult delete_MenuCategory(int id) {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            var data = db.MenuCategories.Find(id);
            return View(data);
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
            db.MenuCategories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Menu_Categories");
        }

        //Controller of Menu categories


        //Controller of Menu

        public IActionResult addMenu()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");

            if (catererId == null)
            {
                // If CatererId is not found in session, redirect to login or show error
                return RedirectToAction("CatererLogin", "Login");
            }
            ViewData["CategoryId"] = new SelectList(db.MenuCategories, "CategoryId", "Category");
            return View();
        }

        [HttpPost]
        public IActionResult addMenu(Menu menu, IFormFile file)
        {
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

            db.Menus.Add(menu);
            db.SaveChanges();

            return RedirectToAction("listMenu");
        }



        [HttpGet]
        public IActionResult listMenu()
        {
            var menus = db.Menus.Include(m => m.Category).ToList();
            return View(menus);

        }

        [HttpGet]
        public IActionResult deleteMenu(int id) { 
            var data = db.Menus.Find(id);
            return View(data);
        }

        [HttpPost, ActionName("deleteMenu")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteMenuConfirmed(int id) 
        {
            var Menu = db.Menus.Find(id);
            if (Menu == null) return NotFound();
            db.Menus.Remove(Menu);
            db.SaveChanges();
            return RedirectToAction("listMenu");
        }

        [HttpGet]
        public IActionResult editMenu(int id) 
        {
            var menu = db.Menus.Find(id);
            if (menu == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(db.MenuCategories, "CategoryId", "Category");
            return View(menu);
        }

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
            else
            {
                existingMenu.Image = menu.Image;
            }

            existingMenu.ItemName = menu.ItemName;
            existingMenu.Category = menu.Category;
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

        public  IActionResult AddWorkersType()
        {
            return View();
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

        [HttpGet]
        public IActionResult WorkerType()
        {
            return View(db.WorkerTypes.ToList());
        }


        [HttpGet]
        public IActionResult editWorkerType(int id)
        {
            var workerType = db.WorkerTypes.Find(id);
            if (workerType == null) return NotFound();
            return View(workerType);
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

        [HttpGet]
        public IActionResult deleteWorkerType(int id)
        {
            var workerType = db.WorkerTypes.Find(id);
            if (workerType == null) return NotFound();
            return View(workerType);
        }
        
        [HttpPost, ActionName("deleteWorkerType")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteWorkerTypeConfirmed(int id)
        {
            var workerType = db.WorkerTypes.Find(id);
            if (workerType == null) return NotFound();
            db.WorkerTypes.Remove(workerType);
            db.SaveChanges();
            return RedirectToAction("WorkerType");
        }

        //Controller of WorkerType 

        //Controller of Worker

        public IActionResult AddWorkers()
        {
            ViewData["WorkerTypeId"] = new SelectList(db.WorkerTypes, "WorkerTypeId", "WorkerType1");
            return View();
        }
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
            return View(db.Workers.Include(m=> m.WorkerType).Where(s => s.CatererId == catererId.Value).ToList());
        }

        [HttpGet]
        public IActionResult editWorkers(int id)
        {
            ViewData["WorkerTypeId"] = new SelectList(db.WorkerTypes, "WorkerTypeId", "WorkerType1");

            var worker = db.Workers.Find(id);
            return View(worker);
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

        public IActionResult deleteWorker(int id)
        {
            var worker = db.Workers
                   .Include(w => w.WorkerType) 
                   .FirstOrDefault(w => w.WorkerId == id);

            return View(worker);
        }

        [HttpPost, ActionName("deleteWorker")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteConfirmationWorker(int id)
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

        public IActionResult addRawMaterials()
        {
            return View();
        }

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
                return View(rawMaterial);
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
            var RawMaterial = db.RawMaterials.ToList();
            return View(RawMaterial);
        }


        [HttpGet]
        public IActionResult editRawMaterial(int id)
        {
            var existingMaterial = db.RawMaterials.Find(id);
            if (existingMaterial == null) return NotFound();
            return View(existingMaterial);
        }

        [HttpPost]
        public IActionResult editRawMaterial(RawMaterial rawMaterial, int id)
        {
            var existingMaterial = db.RawMaterials.Find(id);
            if (existingMaterial == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingMaterial.Name = rawMaterial.Name;
                db.SaveChanges();
                return RedirectToAction("RawMaterials");

            }

            return View(existingMaterial);
        }


        [HttpGet]
        public IActionResult detailRawMaterial(int id)
        {
            var existingMaterial = (from data in db.RawMaterials where data.IngredientNo == id select data)
                .FirstOrDefault();

            return View(existingMaterial);
        }

        [HttpGet]
        public IActionResult deleteRawMaterial(int id)
        {
             var existingMaterial = db.RawMaterials.Find(id);
            return View(existingMaterial);
        }

        [HttpPost, ActionName("deleteRawMaterial")]
        [ValidateAntiForgeryToken]
        public IActionResult deletedRawMaterial(int id)
        {
            var existingMaterial = db.RawMaterials.Find(id);
            if(existingMaterial == null) return NotFound();

            db.RawMaterials.Remove(existingMaterial);
            db.SaveChanges() ;
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



        [HttpGet]
        public IActionResult addSuppliers()
        {
            return View();
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

        [HttpGet]
        public IActionResult deleteSuppliers(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost, ActionName("deleteSuppliers")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteSuppliersConfrmed(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if(supplier == null) return NotFound();
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

        [HttpGet]
        public IActionResult editSupplier(int id)
        {
            var supplier = db.Suppliers.Find(id);
            return View(supplier);
        }

        [HttpPost]
        public IActionResult editSupplier(int id, Supplier supplier)
        {
            var existingsupplier = db.Suppliers.Find(id);
            if(existingsupplier == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingsupplier.Name = supplier.Name;
                existingsupplier.Address = supplier.Address;
                existingsupplier.Pincode= supplier.Pincode;
                existingsupplier.Phone = supplier.Phone;
                existingsupplier.Mobile = supplier.Mobile;

                db.SaveChanges();
                return RedirectToAction("Suppliers");
            }
            return View(existingsupplier);
        }


        //Controller of Supplier




        // CREATE - GET
    public IActionResult AddSupplierOrder()
    {
        int? catererId = HttpContext.Session.GetInt32("UserId");
        if (catererId == null)
        {
            return RedirectToAction("CatererLogin", "Login");
        }

            var suppliers = db.Suppliers
                          .Where(s => s.CatererId == catererId.Value)
                          .ToList();

            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Name");
            return View();
    }

    // CREATE - POST
    [HttpPost]
    public IActionResult AddSupplierOrder(SupplierOrder order)
    {
        int? catererId = HttpContext.Session.GetInt32("UserId");
        if (catererId == null)
        {
                Console.WriteLine("Caterer must be login!");

                return RedirectToAction("CatererLogin", "Login");
        }
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Model error on '{key}': {error.ErrorMessage}");
                    }
                }
                Console.WriteLine("Not Saved successfully!");
                Console.WriteLine("Caterer", catererId);
            }

            if (ModelState.IsValid)
            {
                order.CatererId = catererId.Value;
                db.SupplierOrders.Add(order);
                db.SaveChanges();
                Console.WriteLine("Saved successfully!");

                return RedirectToAction("ListSupplierOrder");
            }

            var suppliers = db.Suppliers
                          .Where(s => s.CatererId == catererId.Value)
                          .ToList();

            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Name");

            return View(order);
    }

    // READ - LIST
    public IActionResult ListSupplierOrder()
    {
        int? catererId = HttpContext.Session.GetInt32("UserId");
        if (catererId == null)
        {
            return RedirectToAction("CatererLogin", "Login");
        }

        var orders = db.SupplierOrders
            .Include(o => o.Supplier)
            .Where(o => o.CatererId == catererId.Value)
            .ToList();

        return View(orders);
    }

    // READ - DETAILS
    public IActionResult DetailSupplierOrder(int id)
    {
        var order = db.SupplierOrders
            .Include(o => o.Supplier)
            .FirstOrDefault(o => o.SuppOrderNo == id);

        if (order == null)
            return NotFound();

        return View(order);
    }

    // DELETE - GET
    public IActionResult DeleteSupplierOrder(int id)
    {
        var order = db.SupplierOrders.Find(id);
        if (order == null)
            return NotFound();

        return View(order);
    }

    // DELETE - POST
    [HttpPost, ActionName("DeleteSupplierOrder")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var order = db.SupplierOrders.Find(id);
        if (order == null)
            return NotFound();

        db.SupplierOrders.Remove(order);
        db.SaveChanges();
        return RedirectToAction("ListSupplierOrder");
    }

    // UPDATE - GET
    public IActionResult EditSupplierOrder(int id)
    {
        var order = db.SupplierOrders.Find(id);
        if (order == null)
            return NotFound();

        ViewData["SupplierId"] = new SelectList(db.Suppliers, "SupplierId", "Name");
        return View(order);
    }

    // UPDATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditSupplierOrder(SupplierOrder updatedOrder)
    {
        if (ModelState.IsValid)
        {
            var existingOrder = db.SupplierOrders.Find(updatedOrder.SuppOrderNo);
            if (existingOrder == null)
                return NotFound();

            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.SupplierId = updatedOrder.SupplierId;
            existingOrder.EstimatedAmount = updatedOrder.EstimatedAmount;
            existingOrder.InvoiceDone = updatedOrder.InvoiceDone;

            db.SaveChanges();
            return RedirectToAction("ListSupplierOrder");
        }

        ViewData["SupplierId"] = new SelectList(db.Suppliers, "SupplierId", "Name", updatedOrder.SupplierId);
        return View(updatedOrder);
    }
    }
}
