using SingleTicketing.Models;
using System.Collections.Generic;
namespace SingleTicketing.Data
{
    public class User
    {
        public int Id { get; set; }
        public int? Sts_Id { get; set; }
        public required string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public required string PasswordHash { get; set; }
        public required string RoleName { get; set; }
        public Role? Role { get; set; }
        public string? StatusName { get; set; }
        public Status? Status { get; set; }
        public string? Remarks { get; set; }
        public List<Attachment>? Attachments { get; set; } = new List<Attachment>();
    }
}
