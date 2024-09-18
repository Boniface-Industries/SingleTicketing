using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;

namespace SingleTicketing.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly MyDbContext _context;

        public SuperAdminController(MyDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("Username,PasswordHash,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
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

            // Create a list of roles for the dropdown
            ViewData["Roles"] = new SelectList(new List<string> { "Admin", "Driver", "Enforcer" });

            return View(user);
        }

        // POST: SuperAdmin/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Username,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(user.Id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.Username = user.Username;
                    existingUser.Role = user.Role;
                    // Do not update PasswordHash or other sensitive fields

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User details updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the user.";
                    Console.WriteLine($"Exception: {ex.Message}");
                    return View(user);
                }
            }

            // Log the validation errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }

            // Re-populate roles for the dropdown if model state is invalid
            ViewData["Roles"] = new SelectList(new List<string> { "Admin", "Driver", "Enforcer" });
            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View(user);
        }



     
    }
}
