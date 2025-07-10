using JobApplyBotInMVC.Data;
using JobApplyBotInMVC.Models;
using JobApplyBotInMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplyBotInMVC.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly EmailService _emailService;
        private readonly WhatsAppService _whatsAppService;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public JobApplicationController(EmailService emailService, WhatsAppService whatsAppService, AppDbContext db, IWebHostEnvironment env)
        {
            _emailService = emailService;
            _whatsAppService = whatsAppService;
            _db = db;
            _env = env;
        }

        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(JobApplicationRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "❌ Invalid input.";
                return View();
            }

            if (request.ResumeFile != null && request.ResumeFile.Length > 0)
            {
                string uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid() + Path.GetExtension(request.ResumeFile.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ResumeFile.CopyToAsync(stream);
                }

                request.ResumePath = "/uploads/" + fileName;
            }

            if (request.Medium.ToLower() == "email")
            {
                await _emailService.SendEmail(request);
                ViewBag.Message = "✅ Email sent successfully!";
            }
            else if (request.Medium.ToLower() == "whatsapp")
            {
                string link = _whatsAppService.GenerateWhatsAppMessage(request);
                ViewBag.WhatsAppLink = link;
                ViewBag.Message = "✅ WhatsApp message link generated.";
            }

            await _db.SaveApplicationAsync(request);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Submitted()
        {
            var apps = await _db.GetAllApplicationsAsync();
            return View(apps); // returns to Submitted.cshtml
        }

    }
}
