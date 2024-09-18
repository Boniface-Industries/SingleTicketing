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
    }
}
