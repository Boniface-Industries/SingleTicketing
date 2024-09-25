using SingleTicketing.Data;

namespace SingleTicketing.Models
{
    public class Attachment
    {
        public int? Id { get; set; }
        public byte[]? FileData { get; set; }
        public byte[]? Data { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
