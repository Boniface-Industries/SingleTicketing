using Microsoft.AspNetCore.Mvc;

namespace SingleTicketing.Controllers
{
    public class DriverController : Controller
    { 
        public IActionResult Index()
        {
            // Driver dashboard logic
            return View();
        }

        public IActionResult Scan()
        {
            return View();
        }

        public IActionResult License()
        {
            return View();
        }
        public IActionResult DriverDetails()
        {
            return View();
        }
    }
}
