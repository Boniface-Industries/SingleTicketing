using System.ComponentModel.DataAnnotations;

namespace SingleTicketing.Models
{
    public class EditRoleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Role Name cannot be longer than 100 characters.")]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}
