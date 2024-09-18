using Microsoft.AspNetCore.Mvc;
using SingleTicketing.Data;
using SingleTicketing.Models;
using SingleTicketing.Services;
using System.Diagnostics;

namespace SingleTicketing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly UserService _userService;

        public HomeController(
            ILogger<HomeController> logger,
            MyDbContext context,
            UserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Action to create users
        public IActionResult CreateUser()
        {
            // Example of creating users
            var adminUser = _userService.CreateUser("admin1", "password123", "Admin");
            var driverUser = _userService.CreateUser("driver1", "password123", "Driver");
            var enforcerUser = _userService.CreateUser("enforcer1", "password123", "Enforcer");

            _context.Users.AddRange(adminUser, driverUser, enforcerUser);
            _context.SaveChanges();

            return Content("Users created successfully!");
        }
    }
}
