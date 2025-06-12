using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineCatering.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace OnlineCatering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Catering Site", _config["EmailSettings:FromEmail"]));
                emailMessage.To.Add(new MailboxAddress("Admin", _config["EmailSettings:ToEmail"]));
                emailMessage.Subject = "New Contact Form Submission";

                emailMessage.Body = new TextPart("html")
                {
                    Text = $@"
<div style=""background-color:#FFFCF8; padding: 40px; font-family: Arial, sans-serif; color: #000;"">
  <div style=""max-width: 600px; margin: auto; background-color: #fff; border-radius: 12px; box-shadow: 0 0 10px rgba(212, 167, 98, 0.2); overflow: hidden;"">
    
    <!-- Header -->
    <div style=""background-color: #D4A762; padding: 24px 32px; color: white; text-align: center;"">
      <h1 style=""margin: 0; font-size: 24px;"">Thank You for Contacting Us</h1>
      <p style=""margin: 4px 0 0; font-size: 16px;"">We appreciate your message</p>
    </div>

    <!-- Content -->
    <div style=""padding: 32px;"">
      <h2 style=""font-size: 20px; color: #D4A762;"">Message Details</h2>
      <p style=""font-size: 16px; line-height: 1.6; color: #808080;"">
        <strong>Name:</strong> {{{{Name}}}}<br>
        <strong>Email:</strong> {{{{Email}}}}<br>
        <strong>Subject:</strong> {{{{Subject}}}}<br><br>
        {{{{MessageBody}}}}
      </p>
    </div>

    <!-- Footer -->
    <div style=""background-color: #F9F5F0; padding: 16px 32px; text-align: center; font-size: 12px; color: #808080;"">
      &copy; @DateTime.Now.Year Online Catering Platform. All rights reserved.
    </div>
  </div>
</div>"
                };


                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _config["EmailSettings:Host"],
                        int.Parse(_config["EmailSettings:Port"]),
                        MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(
                        _config["EmailSettings:UserName"],
                        _config["EmailSettings:Password"]);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                TempData["Message"] = "Thank you! Your message has been sent.";
                return RedirectToAction("Contact");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
