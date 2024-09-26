﻿using SingleTicketing.Data;

namespace SingleTicketing.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        // Foreign key for User
        public int UserId { get; set; }  // Use int to match the primary key type in User

        // Navigation property
        public virtual required User User { get; set; }
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public required string Action { get; set; }
        public required string Details { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string IpAddress { get; set; }
    }

}
