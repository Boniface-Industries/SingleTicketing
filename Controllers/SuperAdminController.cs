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

        public async Task<IActionResult> Audit(int? page, DateTime? filterDate)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var auditTrails = _context.AuditTrails.AsQueryable();

            // Apply the date filter if provided
            if (filterDate.HasValue)
            {
                auditTrails = auditTrails.Where(a => a.Timestamp.Date == filterDate.Value.Date);
            }

            auditTrails = auditTrails.OrderByDescending(a => a.Timestamp);

            // Get the total count of audit trails
            int totalCount = await auditTrails.CountAsync();

            // Fetch the specific page of results
            var pagedAuditTrails = await auditTrails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Create a view model or directly pass the paged results to the view
            var viewModel = new AuditViewModel
            {
                AuditTrails = pagedAuditTrails,
                PageNumber = pageNumber,
                TotalCount = totalCount,
                PageSize = pageSize,
                FilterDate = filterDate
            };

            return View(viewModel);
        }





        // GET: SuperAdmin/Index
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            try
            {
                // Get total number of users
                var totalUsers = _context.Users.Count();

                // Calculate the users to retrieve for the current page
                var users = _context.Users
                    .Skip((page - 1) * pageSize)  // Skip users from previous pages
                    .Take(pageSize)                // Take the specified number of users
                    .ToList();

                // Create a model to pass to the view that includes users and pagination info
                var model = new UserListViewModel
                {
                    Users = users,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize),
                    SuccessMessage = TempData["SuccessMessage"]?.ToString() // Retrieve success message from TempData
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
            // Ensure available roles are populated before validation
            model.AvailableRoles = await _context.Roles.Select(r => r.RoleName).ToListAsync();

            if (ModelState.IsValid)
            {
                // Validate that PasswordHash is not null or empty
                if (string.IsNullOrWhiteSpace(model.PasswordHash))
                {
                    ModelState.AddModelError("PasswordHash", "Password is required.");
                    return View(model); // Redisplay the form with the error
                }

                // Create a new user instance with additional fields
                var user = new User
                {
                    Sts_Id = model.Sts_Id,
                    Username = model.Username,
                    RoleName = model.RoleName,
                    StatusName = model.StatusName,
                    PasswordHash = _passwordHasher.HashPassword(null, model.PasswordHash), // Hash the password here

                    // Add new fields
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Remarks = model.Remarks,
                    Attachments = new List<Attachment>() // Change to List<Attachment>
                };

                // Process each uploaded file and convert it to Attachment
                if (model.Attachments != null && model.Attachments.Count > 0)
                {
                    foreach (var file in model.Attachments)
                    {
                        if (file.Length > 0) // Ensure the file is not empty
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);
                                var attachment = new Attachment
                                {
                                    FileData = memoryStream.ToArray(),
                                    FileName = file.FileName,
                                    ContentType = file.ContentType,
                                    UserId = user.Id // Assuming you're setting the UserId after the user is created
                                };
                                user.Attachments.Add(attachment); // Add the attachment to the user's attachments
                            }
                        }
                    }
                }

                // Add the user to the database context
                _context.Add(user);

                // Save changes to the database
                await _context.SaveChangesAsync();

                TempData["Success"] = "User created successfully.";
                return RedirectToAction("Index", "SuperAdmin");
            }
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
            var user = await _context.Users.Include(u => u.Attachments).FirstOrDefaultAsync(u => u.Id == id); // Make sure to include Attachments
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Id = user.Id,
                Sts_Id = user.Sts_Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RoleName = user.RoleName,
                StatusName = user.StatusName,

                // Include the new fields
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Remarks = user.Remarks,
                ExistingAttachments = user.Attachments, // This should now match

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
                    var existingUser = await _context.Users.Include(u => u.Attachments).FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update only the relevant fields if they are not null
                    existingUser.Sts_Id = model.Sts_Id ?? existingUser.Sts_Id; // Use ?? to retain existing value if null
                    existingUser.Username = string.IsNullOrEmpty(model.Username) ? existingUser.Username : model.Username;
                    existingUser.PasswordHash = string.IsNullOrEmpty(model.PasswordHash) ? existingUser.PasswordHash : _passwordHasher.HashPassword(existingUser, model.PasswordHash);
                    existingUser.RoleName = string.IsNullOrEmpty(model.RoleName) ? existingUser.RoleName : model.RoleName;
                    existingUser.StatusName = string.IsNullOrEmpty(model.StatusName) ? existingUser.StatusName : model.StatusName;

                    // Update the new fields
                    existingUser.FirstName = string.IsNullOrEmpty(model.FirstName) ? existingUser.FirstName : model.FirstName;
                    existingUser.LastName = string.IsNullOrEmpty(model.LastName) ? existingUser.LastName : model.LastName;
                    existingUser.MiddleName = string.IsNullOrEmpty(model.MiddleName) ? existingUser.MiddleName : model.MiddleName;
                    existingUser.Remarks = string.IsNullOrEmpty(model.Remarks) ? existingUser.Remarks : model.Remarks;

                    // Adding new attachments
                    if (model.Attachments != null && model.Attachments.Count > 0)
                    {
                        foreach (var file in model.Attachments)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);
                                var attachment = new Attachment
                                {
                                    Data = memoryStream.ToArray(), // This should work now
                                    FileName = file.FileName, // Optional: capture the original file name
                                    ContentType = file.ContentType // Optional: capture the file content type
                                };
                                existingUser.Attachments.Add(attachment); // Now add the Attachment object
                            }
                        }
                    }


                    // Update the user in the context
                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessEdit"] = "User details updated successfully.";
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


        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(int attachmentId, int userId)
        {
            // Find the attachment by ID and delete it
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attachment deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Attachment not found.";
            }

            return RedirectToAction("Edit", new { id = userId });
        }


    }


}

