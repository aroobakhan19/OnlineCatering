using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCatering.Models;

namespace OnlineCatering.Controllers
{
    public class MessageController : Controller
    {
        OnlineCateringContext db = new OnlineCateringContext();

        public IActionResult Chat()
        {
            return View();
        }

        // GET: List of Customers who messaged Caterer
        [HttpGet]
        public IActionResult GetCustomerThreads()
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null)
            {
                return Json(new { error = "Not logged in" });
            }

            var messages = db.Messages
                .Include(m => m.FromCustomer)
                .Include(m => m.ToCustomer)
                .Where(m => m.ToCatererId == catererId || m.FromCatererId == catererId)
                .AsEnumerable() // Switch to client-side
                .Select(m => m.FromCustomer ?? m.ToCustomer)
                .Where(c => c != null)
                .DistinctBy(c => c.Id) // Avoid duplicates
                .Select(c => new
                {
                    id = c.Id,
                    name = c.UserName,
                    email = c.Email
                })
                .ToList();

            return Json(messages);
        }

        // GET: All messages between caterer and one customer
        [HttpGet]
        public IActionResult GetMessagesWithCustomer(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null) return Unauthorized();

            var messages = db.Messages
                .Where(m =>
                    (m.FromCustomerId == id && m.ToCatererId == catererId) ||
                    (m.FromCatererId == catererId && m.ToCustomerId == id))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    id = m.Id,
                    content = m.Content,
                    sentAt = m.SentAt.ToString("yyyy-MM-dd hh:mm tt"),
                    isFromCaterer = m.FromCatererId == catererId
                })
                .ToList();

            return Json(messages);
        }

        // POST: Send a message from caterer
        [HttpPost]
        public IActionResult SendMessage(int toCustomerId, string content)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null) return Unauthorized();

            var message = new Message
            {
                FromCatererId = catererId,
                ToCustomerId = toCustomerId,
                Content = content,
                SentAt = DateTime.Now
            };

            db.Messages.Add(message);
            db.SaveChanges();
            return Ok();
        }

        // GET: Delete a message
        [HttpPost]
        public IActionResult Delete_Message(int id)
        {
            int? catererId = HttpContext.Session.GetInt32("UserId");
            if (catererId == null) return Unauthorized();

            var msg = db.Messages.FirstOrDefault(m => m.Id == id && m.FromCatererId == catererId);
            if (msg != null)
            {
                db.Messages.Remove(msg);
                db.SaveChanges();
                return Ok(); // ✅ Return OK to prevent redirect
            }

            return NotFound();
        }



        //Customer Side


        public IActionResult ChatCustomer()
        {
            return View();
        }

        // GET: List of Caterers who messaged Customer
        [HttpGet]
        public IActionResult GetCatererThreads()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                return Json(new { error = "Not logged in" });
            }

            var caterers = db.Messages
                .Include(m => m.FromCaterer)
                .Include(m => m.ToCaterer)
                .Where(m => m.ToCustomerId == customerId || m.FromCustomerId == customerId)
                .AsEnumerable() // Client-side evaluation to avoid complex SQL issues
                .Select(m => m.FromCaterer ?? m.ToCaterer)
                .Where(c => c != null)
                .DistinctBy(c => c.Id)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Restaurant,  // Or c.UserName if you want username
                    email = c.Email
                })
                .ToList();

            return Json(caterers);
        }

        // GET: All messages between customer and one caterer
        [HttpGet]
        public IActionResult GetMessagesWithCaterer(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null) return Unauthorized();

            var messages = db.Messages
                .Where(m =>
                    (m.FromCustomerId == customerId && m.ToCatererId == id) ||
                    (m.FromCatererId == id && m.ToCustomerId == customerId))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    id = m.Id,
                    content = m.Content,
                    sentAt = m.SentAt.ToString("yyyy-MM-dd hh:mm tt"),
                    isFromCustomer = m.FromCustomerId == customerId
                })
                .ToList();

            return Json(messages);
        }

        // POST: Send message from customer to caterer
        [HttpPost]
        public IActionResult SendMessageToCaterer(int toCatererId, string content)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null) return Unauthorized();

            var message = new Message
            {
                FromCustomerId = customerId,
                ToCatererId = toCatererId,
                Content = content,
                SentAt = DateTime.Now
            };

            db.Messages.Add(message);
            db.SaveChanges();
            return Ok();
        }

        // GET: Delete message by customer (only their own messages)
        [HttpPost]
        public IActionResult Delete_Message_Customer(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null) return Unauthorized();

            var msg = db.Messages.FirstOrDefault(m => m.Id == id && m.FromCustomerId == customerId);
            if (msg != null)
            {
                db.Messages.Remove(msg);
                db.SaveChanges();
                return Ok(); // ✅ Return OK to prevent redirect
            }

            return NotFound(); // or any customer chat view you create
        } 

    }
}
