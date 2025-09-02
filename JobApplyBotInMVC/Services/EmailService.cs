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
            string rawBody = request.Position.ToLower() switch
            {
                "fullstack" => GenerateFullstackMessage(request),
                "backend" => GenerateBackendMessage(request),
                "frontend" => GenerateFrontendMessage(request),
                _ => GenerateGenericMessage(request)
            };

            string body = WrapInHtml(rawBody);

            using var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage(fromEmail, request.Contact, subject, body)
            {
                IsBodyHtml = true
            };

            bool attached = false;

            // Attach user-uploaded resume if available
            if (!string.IsNullOrWhiteSpace(request.ResumePath))
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", request.ResumePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    mail.Attachments.Add(new Attachment(fullPath));
                    attached = true;
                }
            }

            // If no resume was attached, try to attach default from local wwwroot
            if (!attached)
            {
                var localResumePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resume.pdf");

                if (File.Exists(localResumePath))
                {
                    mail.Attachments.Add(new Attachment(localResumePath));
                    attached = true;
                }
                else
                {
                    // Optional: Last-resort fallback to try downloading (but won't crash if fails)
                    try
                    {
                        string defaultResumeUrl = "https://sanoof-portfolio.vercel.app/resume.pdf";
                        using var httpClient = new HttpClient();
                        var resumeStream = await httpClient.GetStreamAsync(defaultResumeUrl);
                        var attachment = new Attachment(resumeStream, "resume.pdf", "application/pdf");
                        mail.Attachments.Add(attachment);
                        attached = true;
                    }
                    catch (Exception ex)
                    {
                        // Log error, but don't stop sending the email
                        Console.WriteLine($"[EmailService] Could not attach default resume: {ex.Message}");
                    }
                }
            }

            await smtpClient.SendMailAsync(mail);
        }

        private string WrapInHtml(string content)
        {
            return $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            font-size: 15px;
            color: #333;
            line-height: 1.6;
            background-color: #f4f4f4;
            padding: 20px;
        }}
        .container {{
            background: #fff;
            padding: 20px 25px;
            border-radius: 8px;
            max-width: 650px;
            margin: auto;
            box-shadow: 0 2px 10px rgba(0,0,0,0.08);
        }}
        p {{ margin: 8px 0; }}
        a {{
            color: #007BFF;
            text-decoration: none;
        }}
        a:hover {{ text-decoration: underline; }}
        ul {{ padding-left: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        {content}
    </div>
</body>
</html>";
        }

        private string GenerateFullstackMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName)
                ? "your organization"
                : request.CompanyName;

            return $@"
<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I am excited to apply for the <strong>Full Stack Developer</strong> role at <strong>{company}</strong>. 
I build scalable, high-quality applications using <strong>.NET Core</strong> (backend) and <strong>React.js</strong> (frontend), ensuring both performance and user experience excellence.</p>

<p><strong>Key Skills:</strong> .NET Core, C#, React.js, Tailwind CSS, Entity Framework, SQL Server, REST API Development, Dapper, Git/GitHub, CI/CD, Render, Cloud Deployment, JWT Authentication.</p>

<p>Selected Projects:</p>
<ul>
    <li>ADOTZEE – Admission assistance platform</li>
    <li>Plashoe – Complete E-commerce project</li>
    <li>MediConnect – Healthcare communication platform</li>
    <li>Carple (ongoing) – Real-time driver booking & carpooling system</li>
</ul>

<p>Portfolio: <a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a></p>

<p>I’d be happy to discuss how I can contribute to <strong>{company}</strong>’s development goals.</p>

<p>Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>";
        }

        private string GenerateBackendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName)
                ? "your esteemed organization"
                : request.CompanyName;

            return $@"
<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I’m applying for the <strong>Backend Developer (.NET)</strong> position at <strong>{company}</strong>. 
I specialize in building secure, high-performance REST APIs using <strong>.NET Core</strong>, <strong>Entity Framework</strong>, <strong>SQL Server</strong>, <strong>Dapper</strong>, and <strong>Cloud Hosting</strong>.</p>

<p><strong>Key Skills:</strong> API Design, Database Optimization, ADO.NET, Render, JWT Authentication, Git/GitHub, Docker Basics.</p>

<p>Key Highlights:</p>
<ul>
    <li>Optimized backend performance for large-scale systems</li>
    <li>Developed scalable, secure APIs for production use</li>
    <li>Maintained clean, modular architecture</li>
</ul>

<p>Portfolio: <a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a></p>

<p>I look forward to contributing to <strong>{company}</strong>’s backend excellence.</p>

<p>Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>";
        }

        private string GenerateFrontendMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName)
                ? "your organization"
                : request.CompanyName;

            return $@"
<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I’m excited to apply for the <strong>Frontend Developer</strong> position at <strong>{company}</strong>. 
I create responsive, accessible, and visually engaging UIs using <strong>React</strong>, <strong>Tailwind CSS</strong>, and <strong>Redux Toolkit</strong>.</p>

<p><strong>Key Skills:</strong> JavaScript (ES6+), React, TypeScript, Redux Toolkit, Tailwind CSS, API Integration, Responsive Design, Git/GitHub, Figma to Code, Performance Optimization, Accessibility (WCAG).</p>

<p>Highlighted Work:</p>
<ul>
    <li>Reusable, modular UI components</li>
    <li>Pixel-perfect, mobile-first designs</li>
    <li>High-performance, SEO-friendly pages</li>
</ul>

<p>Portfolio: <a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a></p>

<p>I’d love to bring my frontend expertise to <strong>{company}</strong>.</p>

<p>Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>";
        }

        private string GenerateGenericMessage(JobApplicationRequest request)
        {
            string company = string.IsNullOrWhiteSpace(request.CompanyName)
                ? "your organization"
                : request.CompanyName;

            return $@"
<p>Dear Hiring Team at <strong>{company}</strong>,</p>

<p>I’m applying for the <strong>{request.Position}</strong> position at <strong>{company}</strong>. 
I’m a results-driven developer skilled in <strong>.NET Core</strong>, <strong>React</strong>, and <strong>SQL Server</strong>, 
focused on delivering efficient and reliable solutions.</p>

<p><strong>Core Skills:</strong> .NET Core, C#, React.js, Tailwind CSS, SQL Server, REST APIs, Git/GitHub, Entity Framework, Dapper, Cloud Hosting, JWT Authentication.</p>

<p>Portfolio: <a href='https://sanoof-portfolio.vercel.app'>https://sanoof-portfolio.vercel.app</a></p>

<p>I’d be happy to discuss how my skills can support <strong>{company}</strong>’s projects.</p>

<p>Best regards,<br>
<strong>Sanoof Mohammed</strong><br>
📞 +91 7907805626<br>";
        }
    }
}
