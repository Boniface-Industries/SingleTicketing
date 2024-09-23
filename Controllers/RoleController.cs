using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;
using System.Threading.Tasks;

namespace SingleTicketing.Controllers
{
    public class RoleController : Controller
    {
        private readonly MyDbContext _context;

        public RoleController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync(); // Fetch all roles from the database
            return View(roles); // Pass roles to the view
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new Role
                {
                    RoleName = model.RoleName,
                    Description = model.Description
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _context.Roles.FindAsync(model.Id);
                if (role == null)
                {
                    return NotFound();
                }

                role.RoleName = model.RoleName;
                role.Description = model.Description;

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
