namespace SingleTicketing.Models
{
    public class CreateUserViewModel
    {
       
        public required string Username { get; set; }

    
        public required string PasswordHash { get; set; }

       
        public required string Role { get; set; }
    }
}
