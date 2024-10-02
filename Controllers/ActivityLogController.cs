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
            // Retrieve all activity logs, ordered by descending timestamp (newest first)
            var logs = await _context.ActivityLogs
                .OrderByDescending(l => l.Date) // Sort by Date in descending order
                .ToListAsync();

            return View(logs); // Pass the logs to the view
        }
   
    }
}
