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
        // GET: /Admin/ManageUser/5
        public IActionResult ManageUser(int id)
        {
            var user = _context.Users.Find(id); // Fetch user by ID
            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            }
            return View(user); // Return view with the user model
        }

        // POST: /Admin/UpdateUser
        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Update(user); // Update user in the database
                _context.SaveChanges(); // Save changes to the database
                return RedirectToAction("Index"); // Redirect to another action
            }
            return View(user); // Return view with validation errors
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
