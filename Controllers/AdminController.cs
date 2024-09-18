using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using System.Collections.Generic;
using System.Linq;

namespace SingleTicketing.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;

        // Constructor to inject the database context
        public AdminController(MyDbContext context)
        {
            _context = context;
        }
    
       
        public IActionResult Index()
        {
            try
            {
                var users = _context.Users.ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                // Log the exception and handle it
                TempData["ErrorMessage"] = "An error occurred while fetching the users.";
                return View(new List<User>()); // Return an empty list to avoid null reference
            }
        }

        [HttpPost]
        public IActionResult Login(string licenseNumber, string password)
        {
            // Authenticate admin (this is just an example, use proper authentication logic)
            if (licenseNumber == "admin" && password == "adminpassword")
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Login");
        }
    }

}
