using SingleTicketing.Data;

namespace SingleTicketing.Models
{
    public class UserListViewModel
    {
        public IEnumerable<User>? Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SuccessMessage { get; set; }
    }

}
