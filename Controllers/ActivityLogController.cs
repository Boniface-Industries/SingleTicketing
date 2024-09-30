using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;

namespace SingleTicketing.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly MyDbContext _context;
        public ActivityLogController(MyDbContext context)
        {
            _context = context;
        }
        // GET: /ActivityLog/Index
        public async Task<IActionResult> Index()
        {
            // Retrieve all activity logs, ordered by ascending timestamp
            var logs = await _context.ActivityLogs
                .OrderBy(l => l.Date) // Sort by Timestamp in ascending order
                .ToListAsync();

            return View(logs); // Pass the logs to the view
        }

    }
}
