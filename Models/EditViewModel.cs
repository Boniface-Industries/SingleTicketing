using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace SingleTicketing.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public int? Sts_Id { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }

        public string? PasswordHash { get; set; }
        public string? RoleName { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public string? StatusName { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Remarks { get; set; }
        // This is the new property for file attachments
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
        public List<Attachment> ExistingAttachments { get; set; } = new List<Attachment>(); // For file uploads


    }
}
