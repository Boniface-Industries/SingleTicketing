using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;
using Microsoft.Extensions.Logging;

namespace SingleTicketing.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ILogger<SuperAdminController> _logger;
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
        public SuperAdminController(MyDbContext context, ILogger<SuperAdminController> logger)
        {
            _context = context;
            _logger = logger;
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
        public IActionResult UpdateUser(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.Find(model.Id);
                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "User not found.";
                        return RedirectToAction("ManageUser");
                    }

                    user.Role = model.Role;  // Update role
                    _context.Update(user);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "User details updated successfully.";

                    // Redirect to the ManageUser page with the user ID to avoid form resubmission errors
                    return RedirectToAction("ManageUser", new { id = model.Id });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the user.";
                    _logger.LogError(ex, "Error updating user with Id {UserId}", model.Id);
                }
            }
            else
            {
                // Log model state errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogError("ModelState invalid: {Errors}", string.Join(", ", errors));

                // Re-populate the roles if ModelState is invalid
                model.Roles = GetRolesList();

                TempData["ErrorMessage"] = "Please correct the errors in the form.";
            }

            // Return the same view if the ModelState is invalid
            return View("ManageUser", model);
        }


        private List<SelectListItem> GetRolesList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "Driver", Text = "Driver" },
        new SelectListItem { Value = "SuperAdmin", Text = "Super Admin" },
        new SelectListItem { Value = "Enforcer", Text = "Enforcer" },
        new SelectListItem { Value = "Admin", Text = "Admin" }
    };
        }

    }
}

