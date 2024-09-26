using SingleTicketing.Models;
using SingleTicketing.Data;  
namespace SingleTicketing.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly MyDbContext _context;

        public ActivityLogService(MyDbContext context)
        {
            _context = context;
        }

        public async Task LogActivityAsync(int userId, string action, string details, string ipAddress)
        {
            // Fetch the User entity from the database using the userId
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            // Create the activity log with the required User property
            var log = new ActivityLog
            {
                User = user, // Set the required User object
                UserId = userId, // Use the userId directly since it's of type int
                Username = user.Username ?? "Unknown", // Default to "Unknown" if Username is null
                FirstName = user.FirstName ?? "No First Name", // Default to "No First Name" if null
                LastName = user.LastName ?? "No Last Name", // Default to "No Last Name" if null
                MiddleName = user.MiddleName ?? "No Middle Name", // Default to "No Middle Name" if null
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }




    }
}
