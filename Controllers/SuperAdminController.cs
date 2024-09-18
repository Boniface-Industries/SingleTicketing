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
        public SuperAdminController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult ManageUser(int id)
        {
            var user = _context.Users.Find(id); // Fetch user by ID
            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            }

            var viewModel = new UpdateViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Driver", Text = "Driver" },
                new SelectListItem { Value = "SuperAdmin", Text = "Super Admin" },
                new SelectListItem { Value = "Enforcer", Text = "Enforcer" },
                new SelectListItem { Value = "Admin", Text = "Admin" }
            }
            };

            return View(viewModel); // Return view with the UserViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(UpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = _context.Users.Find(viewModel.Id); // Find the user by Id
                    if (existingUser == null)
                    {
                        TempData["ErrorMessage"] = "User not found.";
                        return View("ManageUser", viewModel); // Return to the same view
                    }

                    existingUser.Username = viewModel.Username;
                    existingUser.Role = viewModel.Role;
                    // Do not update PasswordHash or other fields that should not be modified

                    _context.Update(existingUser); // Update the user in the database
                    _context.SaveChanges(); // Save changes to the database

                    TempData["SuccessMessage"] = "User details updated successfully.";
                    return RedirectToAction("Index"); // Redirect to the Index action
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the user.";
                    return View("ManageUser", viewModel); // Return the same view with the current user data
                }
            }

            TempData["ErrorMessage"] = "Please correct the errors in the form.";
            return View("ManageUser", viewModel); // Return the same view with validation errors
        }
    }
}

