namespace SingleTicketing.Models
{
    public class CreateUserViewModel
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? RoleName { get; set; } // Add this
        public string? StatusName { get; set; } // Add this

        public List<string> AvailableRoles { get; set; } = new List<string>();
    }

}
