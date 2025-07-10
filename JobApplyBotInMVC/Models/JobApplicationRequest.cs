namespace JobApplyBotInMVC.Models
{
    public class JobApplicationRequest
    {
        public int Id { get; set; } // DB Id
        public string? CompanyName { get; set; } // instead of HRName

        public string Contact { get; set; } // Email or WhatsApp
        public string Position { get; set; } // fullstack / backend / frontend
        public string Medium { get; set; } // "email" or "whatsapp"
        public string? ResumePath { get; set; } // Saved filename path

        // Not mapped to DB - only for form upload
        public IFormFile? ResumeFile { get; set; }
    }
}
