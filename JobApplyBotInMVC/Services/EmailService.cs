using JobApplyBotInMVC.Models;
using System.Net.Mail;
using System.Net;

namespace JobApplyBotInMVC.Services
{
    public class EmailService
    {
        public async Task SendEmail(JobApplicationRequest request)
        {
            var fromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM");
            var fromPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            var host = Environment.GetEnvironmentVariable("EMAIL_HOST");
            var port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "587");

            string subject = $"Application for {request.Position} Developer Role";
            string body = request.Position.ToLower() switch
            {
                "fullstack" => GenerateFullstackMessage(request),
                "backend" => GenerateBackendMessage(request),
                "frontend" => GenerateFrontendMessage(request),
                _ => GenerateGenericMessage(request)
            };

            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage(fromEmail, request.Contact, subject, body);

            bool attached = false;

            // Check if uploaded resume exists
            if (!string.IsNullOrWhiteSpace(request.ResumePath))
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", request.ResumePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    mail.Attachments.Add(new Attachment(fullPath));
                    attached = true;
                }
            }

            // If no uploaded resume, fetch and attach from portfolio URL
            if (!attached)
            {
                string defaultResumeUrl = "https://sanoof-portfolio.vercel.app/resume.pdf";

                using var httpClient = new HttpClient();
                var resumeStream = await httpClient.GetStreamAsync(defaultResumeUrl);

                var attachment = new Attachment(resumeStream, "resume.pdf", "application/pdf");
                mail.Attachments.Add(attachment);
            }

            await smtpClient.SendMailAsync(mail);
        }

        private string GenerateFullstackMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your organization" : request.CompanyName;
            return $@"Dear Hiring Team at {company},

I hope this message finds you well.

I am writing to apply for the Full Stack Developer position at {company}. I have hands-on experience in developing scalable applications using .NET Core (C#) for backend and React with Tailwind CSS for frontend. I have successfully delivered production-grade applications like ADOTZEE and Plashoe.

Please find my resume attached for your consideration. You may also visit my portfolio to review my work:  
https://sanoof-portfolio.vercel.app

I would appreciate the opportunity to further discuss how my skills align with your team's goals.

Thank you for your time and consideration.

Kind regards,  
Sanoof Mohammed  
+91 7907805626";
        }

        private string GenerateBackendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;
            return $@"Dear Hiring Team at {company},

I am reaching out to express my interest in the Backend Developer (.NET) role at {company}. My core expertise includes building secure and scalable REST APIs using .NET Core, Entity Framework, ADO.NET, and SQL Server.

My previous work includes platforms such as ADOTZEE and MediConnect, where I focused on building clean, maintainable backend systems.

I have attached my resume for your review. My portfolio is available at:  
https://sanoof-portfolio.vercel.app

I look forward to the opportunity to contribute to your backend team.

Sincerely,  
Sanoof Mohammed  
+91 7907805626";
        }

        private string GenerateFrontendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;
            return $@"Dear Hiring Team at {company},

I am writing to apply for the Frontend Developer position at {company}. I specialize in building responsive, accessible, and modern web applications using React, Tailwind CSS, and Redux Toolkit.

Projects like ADOTZEE and MediConnect showcase my ability to deliver highly polished frontend systems in real-world environments.

Please find my resume attached and feel free to review my portfolio:  
https://sanoof-portfolio.vercel.app

I am eager to bring my frontend expertise to your team.

Best regards,  
Sanoof Mohammed  
+91 7907805626";
        }

        private string GenerateGenericMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;
            return $@"Dear Hiring Team at {company},

I am reaching out to express my interest in the {request.Position} position.

Please find my resume attached. You can also visit my portfolio at:  
https://sanoof-portfolio.vercel.app

I would be glad to discuss how my background can align with your requirements.

Thank you for your consideration.

Warm regards,  
Sanoof Mohammed  
+91 7907805626";
        }

    }
}
