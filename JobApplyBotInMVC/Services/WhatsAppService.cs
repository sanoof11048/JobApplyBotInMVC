using JobApplyBotInMVC.Models;

namespace JobApplyBotInMVC.Services
{
    public class WhatsAppService
    {
        public string GenerateWhatsAppMessage(JobApplicationRequest request)
        {
            string resumeLink = "https://sanoof-portfolio.vercel.app/resume.pdf";

            string message = request.Position.ToLower() switch
            {
                "fullstack" => $@"Hello, I’m interested in the Full Stack Developer role at {request.CompanyName ?? "your company"}. Skilled in React, .NET Core, and SQL Server.  
Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app  
– Sanoof Mohammed  
+91 7907805626",

                "backend" => $@"Hello, I’m interested in the Backend Developer (.NET) role at {request.CompanyName ?? "your company"}. Skilled in .NET Core and SQL Server.  
Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app  
– Sanoof Mohammed  
+91 7907805626",

                "frontend" => $@"Hello, I’m interested in the Frontend Developer role at {request.CompanyName ?? "your company"}. Skilled in JavaScript, TypeScript, React, and Tailwind CSS.  
Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app  
– Sanoof Mohammed  
+91 7907805626",

                _ => $@"Hello, I’m applying for the {request.Position} role at {request.CompanyName ?? "your company"}.  
Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app  
– Sanoof Mohammed  
+91 7907805626"
            };

            string encoded = Uri.EscapeDataString(message);
            return $"https://wa.me/{request.Contact}?text={encoded}";
        }
    }
}
