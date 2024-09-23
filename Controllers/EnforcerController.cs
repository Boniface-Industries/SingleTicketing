using Microsoft.AspNetCore.Mvc;
using SingleTicketing.Data;
using Microsoft.AspNetCore.Http;

namespace SingleTicketing.Controllers
{
    public class EnforcerController : Controller
    {
        private readonly MyDbContext _context;

        public EnforcerController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve the user ID from the session
            var userIdString = HttpContext.Session.GetString("UserId");

            // Initialize username
            string Username = "Guest"; // Default to "Guest"

            if (int.TryParse(userIdString, out int userId))
            {
                // Fetch user from the database using the converted user ID
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                // Check if user exists and get the username
                if (user != null)
                {
                    Username = user.Username; // Assign the actual username
                }
            }

            ViewBag.WelcomeMessage = $"Welcome, {Username}!";
            return View();
        }
    }
}
