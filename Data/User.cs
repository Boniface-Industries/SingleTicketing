namespace SingleTicketing.Data
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }

        // Role Reference (Assuming it's already implemented as we discussed)
        public required string RoleName { get; set; }  // Reference to the RoleName from the Role table
        public Role? Role { get; set; }  // Optional navigation property

        // Status Reference
        public required string StatusName { get; set; }  // Reference to StatusName from the Status table
        public Status? Status { get; set; }  // Optional navigation property
    }
}
