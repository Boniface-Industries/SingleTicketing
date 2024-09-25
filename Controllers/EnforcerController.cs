using Microsoft.AspNetCore.Mvc;
using SingleTicketing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SingleTicketing.Controllers
{
    public class EnforcerController : Controller
    {
        private readonly MyDbContext _context;

        public EnforcerController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            // Driver dashboard logic
            return View();
        }
        public IActionResult Dashboard()
        {
            // Driver dashboard logic
            return View();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Enforcers.ToListAsync());
        }
        // GET: Enforcer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enforcer = await _context.Enforcers.FirstOrDefaultAsync(e => e.Id == id);
            if (enforcer == null) return NotFound();

            return View(enforcer);
        }

        // GET: Enforcer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enforcer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,FirstName,LastName,MiddleName,BirthDate,Address,Contact_No,Department,Email,Cases,Remarks")] Enforcer enforcer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enforcer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enforcer);
        }

        // GET: Enforcer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enforcer = await _context.Enforcers.FindAsync(id);
            if (enforcer == null) return NotFound();

            return View(enforcer);
        }

        // POST: Enforcer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,FirstName,LastName,MiddleName,BirthDate,Address,Contact_No,Department,Email,Cases,Remarks")] Enforcer enforcer)
        {
            if (id != enforcer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enforcer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnforcerExists(enforcer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(enforcer);
        }

        // GET: Enforcer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enforcer = await _context.Enforcers.FirstOrDefaultAsync(e => e.Id == id);
            if (enforcer == null) return NotFound();

            return View(enforcer);
        }

        // POST: Enforcer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enforcer = await _context.Enforcers.FindAsync(id);
            _context.Enforcers.Remove(enforcer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnforcerExists(int id)
        {
            return _context.Enforcers.Any(e => e.Id == id);
        }
    }
}
