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
                "fullstack" => $@"Hello,

I am reaching out to express my interest in the Full Stack Developer position at {request.CompanyName ?? "your company"}.

I have hands-on experience in developing scalable applications using .NET Core (C#) for backend and React with Tailwind CSS for frontend. I’ve worked on full-scale projects like ADOTZEE and Plashoe, which are both live and production-ready.

Here is my resume for your reference: {resumeLink}

You can also view my portfolio here: https://sanoof-portfolio.vercel.app

Please feel free to reach out if you need any additional information. I look forward to the opportunity to contribute to your team.

Kind regards,  
Sanoof Mohammed  
+91 7907805626",

                "backend" => $@"Hello,

I am writing to express my interest in the Backend Developer (.NET) position at {request.CompanyName ?? "your company"}.

I specialize in building secure, scalable APIs using .NET Core, Entity Framework, and SQL Server. My focus is on writing clean and maintainable backend code. Projects like ADOTZEE and MediConnect demonstrate my ability to deliver robust backend systems.

Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app

Thank you for considering my application. I am open to discussing how I can add value to your backend development team.

Sincerely,  
Sanoof Mohammed  
+91 7907805626",

                "frontend" => $@"Hello,

I am interested in the Frontend Developer position at {request.CompanyName ?? "your company"}.

I bring experience in building user-focused interfaces using React, Tailwind CSS, and Redux Toolkit. My previous work includes fully responsive, accessible web platforms like ADOTZEE and MediConnect.

Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app

Please let me know if you need further information. I would welcome the opportunity to be a part of your frontend team.

Best regards,  
Sanoof Mohammed  
+91 7907805626",

                _ => $@"Hello,

I am reaching out to apply for the {request.Position} role at {request.CompanyName ?? "your company"}.

Resume: {resumeLink}  
Portfolio: https://sanoof-portfolio.vercel.app

Please feel free to contact me for any further information. I appreciate your consideration.

Thank you,  
Sanoof Mohammed  
+91 7907805626"
            };

            string encoded = Uri.EscapeDataString(message);
            return $"https://wa.me/{request.Contact}?text={encoded}";


        }

    }
}
