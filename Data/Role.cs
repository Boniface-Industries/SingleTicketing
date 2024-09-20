namespace SingleTicketing.Data
{
    public class Role
    {
        public int Id { get; set; }
        public required string RoleName { get; set; }  // RoleName is unique
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
