namespace SingleTicketing.Models
{
    public class CreateUserViewModel
    {
        public int? Sts_Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? RoleName { get; set; }
        public string? StatusName { get; set; }

        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public string? MiddleName { get; set; } 
        public string? Remarks { get; set; }
        public List<IFormFile>? Attachments { get; set; }



        public List<string> AvailableRoles { get; set; } = new List<string>();
    }

}
