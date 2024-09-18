using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SingleTicketing.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;

        public AccountController(MyDbContext context)
        {
            _context = context;
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
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            // Set role and username in session
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("Username", user.Username);

            TempData["SuccessMessage"] = "Login successful!";

            // Redirect based on role
            return user.Role switch
            {
                "SuperAdmin" => RedirectToAction("Index", "SuperAdmin"),
                "Admin" => RedirectToAction("Index", "Admin"),
                "Driver" => RedirectToAction("Index", "Driver"),
                "Enforcer" => RedirectToAction("Index", "Enforcer"),
                _ => RedirectToAction("Index", "Home"),
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var hash = HashPassword(enteredPassword);
            return hash == storedHash;
        }
    }
}
