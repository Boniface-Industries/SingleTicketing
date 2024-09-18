using Microsoft.AspNetCore.Mvc.Rendering;

namespace SingleTicketing.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required IEnumerable<SelectListItem> Roles { get; set; }
    }
}
