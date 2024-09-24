using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Services;
using SingleTicketing.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;

namespace SingleTicketing.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly MyDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public SuperAdminController(MyDbContext context, IPasswordHasher<User> passwordHasher, IUserService userService)
        {
            _userService = userService;
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public IActionResult Home()
        {
            // Driver dashboard logic
            return View();
        }
        // GET: SuperAdmin/Index
        public IActionResult Index()
        {
            try
            {
                // Retrieve all users from the database
                var users = _context.Users.ToList();

                // Create a model to pass to the view that includes users
                var model = new UserListViewModel
                {
                    Users = users
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the users.";
                return View(new UserListViewModel { Users = new List<User>() }); // Return an empty list to avoid null reference
            }
        }



        //// GET: SuperAdmin/Delete/5
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: SuperAdmin/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //        await _context.SaveChangesAsync();
        //    }

        //    TempData["SuccessMessage"] = "User deleted successfully.";
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateUserViewModel
            {
                AvailableRoles = await _context.Roles.Select(r => r.RoleName).ToListAsync()
            };
            return View(viewModel);
        }
        // POST: SuperAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new user instance with PasswordHash set in the initializer
                var user = new User
                {
                    Username = model.Username,
                    RoleName = model.RoleName,
                    StatusName = model.StatusName,
                    PasswordHash = _passwordHasher.HashPassword(null, model.PasswordHash) // Hash the password here
                };

                // Add the user to the database context
                _context.Add(user);

                // Save changes to the database
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction(nameof(Index));
            }

            model.AvailableRoles = await _context.Roles.Select(r => r.RoleName).ToListAsync();
            // If we get to this point, something went wrong, so redisplay the form.    
            return View(model);
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
                RoleName = user.RoleName,
                StatusName = user.StatusName,

                AvailableRoles = await _context.Roles.Select(r => r.RoleName).ToListAsync()
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

                    if (!string.IsNullOrEmpty(model.RoleName))
                    {
                        existingUser.RoleName = model.RoleName;
                    }


                    if (!string.IsNullOrEmpty(model.StatusName))
                    {
                        existingUser.StatusName = model.StatusName;
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
 
 

            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePassword([FromBody] PasswordValidationModel model)
        {
            // Retrieve the logged-in user's ID from session
            var loggedInUserIdString = HttpContext.Session.GetString("LoggedInUserId");

            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                return Unauthorized(); // Session expired or not logged in
            }

            // Convert the session ID to an integer
            if (!int.TryParse(loggedInUserIdString, out int loggedInUserId))
            {
                return BadRequest("Invalid session User ID");
            }

            // Fetch the logged-in user from the database
            var loggedInUser = await _context.Users.FindAsync(loggedInUserId);
            if (loggedInUser == null)
            {
                return Unauthorized(); // User not found
            }

            // Verify the password for the logged-in user
            var verificationResult = _passwordHasher.VerifyHashedPassword(loggedInUser, loggedInUser.PasswordHash, model.Password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                // Password is correct, allow the editing process to continue
                return Ok(); // You can include any necessary data if needed
            }
            else
            {
                return Unauthorized(); // Password is incorrect
            }
        }




    }


}

