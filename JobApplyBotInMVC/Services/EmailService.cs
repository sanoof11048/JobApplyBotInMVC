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
            mail.IsBodyHtml = true;
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
            return $@"<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I hope you are doing well.</p>

<p>
I'm writing to express my interest in the <strong>Full Stack Developer</strong> role at <strong>{company}</strong>. I specialize in developing scalable and efficient web applications using <strong>.NET Core (C#)</strong> for the backend and <strong>React.js</strong> with <strong>Tailwind CSS</strong> for the frontend. My expertise spans API development, database design, clean UI/UX implementation, and performance optimization.
</p>

<p>Here are some of the key projects I’ve worked on:</p>
<ul>
    <li><strong>ADOTZEE</strong> – An online admission assistance platform connecting students with colleges.</li>
    <li><strong>Plashoe</strong> – A fully functional shoe e-commerce site with features like cart, wishlist, and order tracking.</li>
    <li><strong>Mediconnect</strong> – A communication system for home nurses and patient relatives, with modules for vitals, food/medication logs, alerts, and chat.</li>
    <li><strong>Carple</strong> (ongoing) – A driver booking and carpooling platform enabling real-time ride management and community-based travel coordination.</li>
</ul>

<p>I focus on writing clean, maintainable code and follow <strong>SOLID principles</strong> and modern architectural standards.</p>

<p>
Please find my resume attached for your consideration. You can also explore my portfolio here:<br>
<a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a>
</p>

<p>I would welcome the opportunity to contribute to <strong>{company}</strong> and am available to join immediately.</p>

<p>Thank you for your time and consideration.</p>

<p>
Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>
✉️ sanoofmohammed.pvt@gmail.com
</p>";
        }



        private string GenerateBackendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;

            return $@"<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I hope you are doing well.</p>

<p>
I'm writing to apply for the <strong>Backend Developer (.NET)</strong> role at <strong>{company}</strong>. My expertise includes building secure and scalable REST APIs using <strong>.NET Core</strong>, <strong>Entity Framework</strong>, <strong>ADO.NET</strong>, and <strong>SQL Server</strong>.
</p>

<p>
I follow best practices in architecture, clean code, and security, and am passionate about backend development and optimization.
</p>

<p>
Please find my resume attached. You can also explore my portfolio here:<br>
<a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a>
</p>

<p>I look forward to the opportunity to contribute to <strong>{company}</strong>.</p>

<p>
Sincerely,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>
✉️ sanoofmohammed.pvt@gmail.com
</p>";
        }



        private string GenerateFrontendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;

            return $@"<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I hope this message finds you well.</p>

<p>
I'm excited to apply for the <strong>Frontend Developer</strong> role at <strong>{company}</strong>. I specialize in building modern, responsive, and accessible web applications using <strong>React.js</strong>, <strong>Tailwind CSS</strong>, and <strong>Redux Toolkit</strong>.
</p>

<p>
My approach emphasizes clean UI/UX, performance optimization, and reusable component-driven architecture.
</p>

<p>
Please find my resume attached. You can also view my portfolio here:<br>
<a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a>
</p>

<p>
I would love the opportunity to contribute to <strong>{company}</strong>'s frontend development efforts.</p>

<p>
Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>
✉️ sanoofmohammed.pvt@gmail.com
</p>";
        }

        private string GenerateGenericMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName) ? "your company" : request.CompanyName;

            return $@"<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>
I am writing to express my interest in the <strong>{request.Position}</strong> position.
</p>

<p>
I am a passionate and results-driven software developer with a strong foundation in full-stack web development using modern technologies like <strong>.NET Core</strong>, <strong>React</strong>, and <strong>SQL</strong>. I follow clean code principles and continuously aim to deliver efficient and reliable software.
</p>

<p>
Please find my resume attached. You can also visit my portfolio:<br>
<a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a>
</p>

<p>
Thank you for considering my application. I would be excited to discuss how I can contribute to <strong>{company}</strong>.
</p>

<p>
Warm regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>
✉️ sanoofmohammed.pvt@gmail.com
</p>";
        }


    }
}
