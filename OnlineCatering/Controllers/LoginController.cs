using Microsoft.AspNetCore.Mvc;
using OnlineCatering.Models;

namespace OnlineCatering.Controllers
{
    public class LoginController : Controller
    {
        OnlineCateringContext db = new OnlineCateringContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();

        }
        [HttpPost]

        public IActionResult Signup(Login lg)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(lg);
                db.SaveChanges();
                // Better UX
            }
            //return View();
            return RedirectToAction("Login");

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        public IActionResult Login(Login lg)
        {
            var user = db.Logins.FirstOrDefault(x =>
                x.UserName == lg.UserName && x.UserPassword == lg.UserPassword);

            if (user != null)
            {
                // You can use Session or Authentication here later
                return RedirectToAction("Index", "Home"); // Or customer dashboard
            }

            ViewBag.Error = "Invalid credentials!";
            return View(lg);
        }

        public IActionResult CatererReg()
        {
            return View();

        }

        [HttpPost]

        public IActionResult CatererReg(CatererLogin cl)
        {
            if (ModelState.IsValid)
            {
                db.CatererLogins.Add(cl);
                db.SaveChanges();
                // Better UX
            }
            return View();

        }

        [HttpGet]
        public IActionResult CatererLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CatererLogin(CatererLogin cl)
        {
            var caterer = db.CatererLogins.FirstOrDefault(x =>
                x.Email == cl.Email && x.Password == cl.Password);

            if (caterer != null)
            {
                // Later: Add session or authentication logic
                return RedirectToAction("Privacy", "Home"); // Or caterer dashboard
            }

            ViewBag.Error = "Invalid login credentials!";
            return View(cl);
        }


    }
}
