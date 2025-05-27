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
            return View(db.MenuCategories.ToList());
        }

        [HttpGet]
        public IActionResult Edit_MenuCategory(int id)
        {
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
            if (ModelState.IsValid)
            {
                db.Update(menuCategory);
                db.SaveChanges();
            }
            return RedirectToAction("Menu_Categories");
        }

        public IActionResult Detail_MenuCategory(int id)
        {
            var t = (from data in db.MenuCategories where data.CategoryId == id select data).FirstOrDefault();

            return View(t);
        }

        [HttpGet]
        public IActionResult delete_MenuCategory(int id) {
            var data = db.MenuCategories.Find(id);
            return View(data);
        }

        [HttpPost, ActionName("delete_MenuCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult deleteConfirmed(int id) { 
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

            if (ModelState.IsValid)
            {
                db.Workers.Add(workers);
                db.SaveChanges();
            }
            return RedirectToAction("Workers");
        }

        public IActionResult Workers()
        {
            return View(db.Workers.Include(m=> m.WorkerType).ToList());
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

    }
}
