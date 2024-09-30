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

            // Get the current date and time
            DateTime now = DateTime.UtcNow;

            // Create the activity log with the required properties
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
                Date = now.Date, // Set the date part
                Time = now.TimeOfDay, // Set the time part
                IpAddress = ipAddress,
                Page = "N/A" // Set a default value for actions not tied to specific pages
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }


        // New method to log page visits, as required by IActivityLogService
        public async Task LogPageVisitAsync(int userId, string pageUrl, string ipAddress)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            // Get the current date and time
            DateTime now = DateTime.UtcNow;

            var log = new ActivityLog
            {
                User = user,
                UserId = userId,
                Username = user.Username ?? "Unknown",
                FirstName = user.FirstName ?? "No First Name",
                LastName = user.LastName ?? "No Last Name",
                MiddleName = user.MiddleName ?? "No Middle Name",
                Action = "Page Visit",
                Details = $"Visited page: {pageUrl}",
                Page = pageUrl, // Set the actual page URL
                Date = now.Date, // Set the date part
                Time = now.TimeOfDay, // Set the time part
                IpAddress = ipAddress
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }

    }



}

