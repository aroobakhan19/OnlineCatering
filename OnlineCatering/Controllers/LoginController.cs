using Microsoft.AspNetCore.Identity;
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
            if(db.Logins.Any(x=> x.Email == lg.Email))
            {
                ModelState.AddModelError("Email", "Email is already Registered");
                return View(lg);
            }

            if (!ModelState.IsValid)
            {                
                return View();
            }
            var passwordhashed = new PasswordHasher<string>();
            lg.UserPassword = passwordhashed.HashPassword(null, lg.UserPassword);

            db.Logins.Add(lg);
            db.SaveChanges();

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
            var user = db.Logins.FirstOrDefault(x => x.Email == lg.Email);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                var result = passwordHasher.VerifyHashedPassword(null, user.UserPassword, lg.UserPassword);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserType", "Customer");
                    Console.WriteLine("Login successful for user: " + user.UserName);
                    // Add session/auth logic later
                    return RedirectToAction("Privacy", "Home");
                }
                else
                {
                    Console.WriteLine("Password verification failed");
                }

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

            if (db.CatererLogins.Any(x => x.Email == cl.Email))
            {
                ModelState.AddModelError("Email", "Email is already Registered");
                return View(cl);
            }

            if (!ModelState.IsValid)
            {
               return View(cl);
                // Better UX
            }
                     

            var passwordhashed = new PasswordHasher<string>();
            cl.Password = passwordhashed.HashPassword(null,cl.Password);

             db.CatererLogins.Add(cl);
                db.SaveChanges();

            return RedirectToAction("CatererLogin");

        }

        [HttpGet]
        public IActionResult CatererLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CatererLogin(CatererLogin cl)
        {
            var caterer = db.CatererLogins.FirstOrDefault(x => x.Email == cl.Email);

            if (caterer != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                var result = passwordHasher.VerifyHashedPassword(null, caterer.Password, cl.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", caterer.Id);
                    HttpContext.Session.SetString("UserName", caterer.Restaurant);
                    HttpContext.Session.SetString("UserEmail", caterer.Email);
                    HttpContext.Session.SetString("UserType", "Caterer");



                    // Add session/auth logic later
                    return RedirectToAction("Privacy", "Home");
                }

            }

            ViewBag.Error = "Invalid login credentials!";
            return View(cl);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



    }
}
