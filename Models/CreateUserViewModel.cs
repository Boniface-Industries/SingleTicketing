namespace SingleTicketing.Models
{
    public class CreateUserViewModel
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public string? RoleName { get; set; } // Add this
        public string? StatusName { get; set; } // Add this
    }

}
