using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SingleTicketing.Services;

namespace SingleTicketing.Controllers
{
    public class AccountController : Controller
    {
        private readonly IActivityLogService _activityLogService;
        private readonly MyDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(MyDbContext context, IActivityLogService activityLogService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _activityLogService = activityLogService;
            _passwordHasher = passwordHasher;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Find the user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            // Debug output
            System.Diagnostics.Debug.WriteLine($"Verifying password for user: {user.Username}");

            // Verify the password
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            // Password is correct, set session for the logged-in user
            HttpContext.Session.SetString("LoggedInUserId", user.Id.ToString());
            HttpContext.Session.SetString("UserRole", user.RoleName);
            HttpContext.Session.SetString("Username", user.Username);

            // Store the user's full name in the session
            HttpContext.Session.SetString("FirstName", user.FirstName ?? "");
            HttpContext.Session.SetString("LastName", user.LastName ?? "");
            HttpContext.Session.SetString("MiddleInitial", !string.IsNullOrEmpty(user.MiddleName) ? user.MiddleName.Substring(0, 1) : "");

            TempData["SuccessMessage"] = "Login successful!";

            // Capture the IPv4 address
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1"; // Use a default value if IP is not available

            // Log the login activity
            await _activityLogService.LogActivityAsync(user.Id, "Login", "User logged in", ipAddress);

            // Redirect based on role
            return user.RoleName switch
            {
                "SuperAdmin" => RedirectToAction("Home", "SuperAdmin"),
                "Admin" => RedirectToAction("Index", "Admin"),
                "Driver" => RedirectToAction("Dashboard", "Driver"),
                "Enforcer" => RedirectToAction("Dashboard", "Enforcer"),
                _ => RedirectToAction("Index", "Home"),
            };
        }

        public async Task<IActionResult> Logout()
        {
            System.Diagnostics.Debug.WriteLine("Logout initiated.");

            // Retrieve the user ID from the session
            var userId = HttpContext.Session.GetString("LoggedInUserId");

            if (!string.IsNullOrEmpty(userId))
            {
                // Capture the IP address
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";

                // Log the logout activity
                try
                {
                    await _activityLogService.LogActivityAsync(int.Parse(userId), "Logout", "User logged out", ipAddress);
                    System.Diagnostics.Debug.WriteLine($"User ID logged out: {userId}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error logging logout activity: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No user ID found in session for logout.");
            }

            // Clear the session
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home"); // Redirect to the home page after logout
        }


    }
}
