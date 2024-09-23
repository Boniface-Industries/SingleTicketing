namespace SingleTicketing.Data
{
    public class Status
    {
        public int Id { get; set; }  // Primary Key
        public string? StatusName { get; set; }  // Status name (Active, Inactive, etc.)
        public string? Description { get; set; }  // Optional description
    }
}
