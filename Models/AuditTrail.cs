namespace SingleTicketing.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public int EntityId { get; set; }  // ID of the affected entity (Enforcer in this case)
        public string FieldName { get; set; } = string.Empty;  // Field that was changed
        public string? OldValue { get; set; }  // Old value of the field
        public string? NewValue { get; set; }  // New value of the field
        public string Action { get; set; } = string.Empty;  // Action performed (Create/Update)
        public string? Username { get; set; }  // User who made the change
        public DateTime Timestamp { get; set; }  // When the change occurred
    }

}
