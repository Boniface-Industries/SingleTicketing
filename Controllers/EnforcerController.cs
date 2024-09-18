using Microsoft.AspNetCore.Mvc;

namespace SingleTicketing.Controllers
{
    public class EnforcerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
