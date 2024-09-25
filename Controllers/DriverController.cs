using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Models;

namespace SingleTicketing.Controllers
{
    public class DriverController : Controller
    {
        private readonly MyDbContext _context;


        public DriverController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Driver dashboard logic
            return View();
        }
        public IActionResult Dashboard()
        {
            // Driver dashboard logic
            return View();
        }
        [HttpPost]
        public IActionResult Login(DriverLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the driver by License Number
                var driver = _context.Drivers.FirstOrDefault(d => d.LicenseNumber == model.LicenseNumber);
                if (driver != null)
                {
                    // Authenticate the driver (you can set up a session or a cookie here)
                    // For example:
                    // HttpContext.Session.SetString("DriverId", driver.Id.ToString());
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError(string.Empty, "Invalid License Number.");
            }
            return View(model);
        }
       
    }
}
