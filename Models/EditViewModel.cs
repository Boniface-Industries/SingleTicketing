﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace SingleTicketing.Models
{
    public class EditViewModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }

        public string? PasswordHash { get; set; }
        public string? RoleName { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public string? StatusName { get; set; }
    }
}
