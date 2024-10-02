namespace SingleTicketing.Models
{
    public class AuditViewModel
    {
        public List<AuditTrail>? AuditTrails { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public DateTime? FilterDate { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
