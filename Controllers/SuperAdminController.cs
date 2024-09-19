using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Helpers;
using SingleTicketing.Models;
using System.Security.Cryptography;
using System.Text;

namespace SingleTicketing.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public SuperAdminController(MyDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: SuperAdmin/Index
        public IActionResult Index()
        {
            try
            {
                var users = _context.Users.ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the users.";
                return View(new List<User>()); // Return an empty list to avoid null reference
            }
        }

        // GET: SuperAdmin/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: SuperAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: SuperAdmin/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: SuperAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    Role = model.Role,
                    PasswordHash = HashPassword(model.Password) // Hash the password before saving
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // If we get to this point, something went wrong, so redisplay the form.    
            return View(model);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        // GET: SuperAdmin/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: SuperAdmin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash, 
                Role = user.Role,
                Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "Driver", Text = "Driver" },
                new SelectListItem { Value = "Enforcer", Text = "Enforcer" }
            }
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update only the relevant fields if they are not null
                    if (!string.IsNullOrEmpty(model.Username))
                    {
                        existingUser.Username = model.Username;
                    }

                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        // Hash the password
                        existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, model.PasswordHash);
                    }

                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        existingUser.Role = model.Role;
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User details updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the user.";
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred.";
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            // Re-populate roles for the dropdown if model state is invalid
            model.Roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "Admin", Text = "Admin" },
        new SelectListItem { Value = "Driver", Text = "Driver" },
        new SelectListItem { Value = "Enforcer", Text = "Enforcer" }
    };

            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View(model);
        }




    }
}
