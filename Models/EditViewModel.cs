using Microsoft.AspNetCore.Mvc.Rendering;

namespace SingleTicketing.Models
{
    public class EditViewModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }

        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
