namespace SingleTicketing.Data
{
    public class Admin
    {

        public int Id { get; set; }

        public required string UserName { get; set; }

        public string? FullName { get; set; }

        public required string Password { get; set; }

        public string? Access_Level { get; set; }

        public string? Email { get; set; }
    }
}
