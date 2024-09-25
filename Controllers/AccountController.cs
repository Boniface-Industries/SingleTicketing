﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SingleTicketing.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(MyDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
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
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            // Debug output
            System.Diagnostics.Debug.WriteLine($"Verifying password for user: {user.Username}");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View(model);
            }

            // Password is correct, now set the session for the logged-in user
            HttpContext.Session.SetString("LoggedInUserId", user.Id.ToString());

            // Set role and username in session
            HttpContext.Session.SetString("UserRole", user.RoleName);
            HttpContext.Session.SetString("Username", user.Username);

            TempData["SuccessMessage"] = "Login successful!";

            // Redirect based on role
            return user.RoleName switch
            {
                "SuperAdmin" => RedirectToAction("Home", "SuperAdmin"),
                "Admin" => RedirectToAction("Index", "Admin"),
                "Driver" => RedirectToAction("Index", "Driver"),
                "Enforcer" => RedirectToAction("Dashboard", "Enforcer"),
                _ => RedirectToAction("Index", "Home"),
            };
        }
        // GET: /Account/Logout
        [HttpPost] // Use POST to prevent CSRF attacks
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "You have been logged out.";

            return RedirectToAction("Index", "Home"); // Redirect to the home page after logout
        }


    }
}
