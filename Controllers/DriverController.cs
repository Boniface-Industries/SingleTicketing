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

        public IActionResult License()
        {
            // Get the driver's ID from session
            var driverId = HttpContext.Session.GetInt32("DriverId");
            if (driverId == null)
            {
                return RedirectToAction("Login"); // Redirect to login if not found
            }

            // Fetch driver by ID
            var driver = _context.Drivers.Find(driverId.Value);
            if (driver == null) return NotFound();

            var viewModel = new DriverViewModel
            {
                Id = driver.Id,
                UserName = driver.UserName,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber,
                Birthdate = driver.Birthdate,
                PlateNumber = driver.PlateNumber,
                LicenseRestrictions = driver.LicenseRestrictions,
                TOP = driver.TOP,
                TypeOfVehicle = driver.TypeOfVehicle,
                Demerit = driver.Demerit,
                Email = driver.Email,
                Remarks = driver.Remarks,
            };

            return View(viewModel);
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
                    // Set the driver's ID in session to use it later
                    HttpContext.Session.SetInt32("DriverId", driver.Id);
                    return RedirectToAction("License", new { id = driver.Id }); // Pass driver ID to License action
                }
                ModelState.AddModelError(string.Empty, "Invalid License Number.");
            }
            return View(model);
        }
        // GET: Drivers
        public IActionResult Index()
        {
            var drivers = _context.Drivers.ToList();
            return View(drivers);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new DriverViewModel());
        }

        // POST: Drivers/Create
        [HttpPost]
        public IActionResult Create(DriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                var driver = new Driver
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    LicenseNumber = model.LicenseNumber,
                    Birthdate = model.Birthdate,
                    PlateNumber = model.PlateNumber,
                    LicenseRestrictions = model.LicenseRestrictions,
                    TOP = model.TOP,
                    TypeOfVehicle = model.TypeOfVehicle,
                    Demerit = model.Demerit,
                    Email = model.Email,
                    Remarks = model.Remarks
                };
                _context.Drivers.Add(driver);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_CreatePartial", model);
        }

        // GET: Drivers/Edit/{id}
        public IActionResult Edit(int id)
        {
            var driver = _context.Drivers.Find(id);
            if (driver == null) return NotFound();

            var model = new DriverViewModel
            {
                Id = driver.Id,
                UserName = driver.UserName,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber,
                Birthdate = driver.Birthdate,
                PlateNumber = driver.PlateNumber,
                LicenseRestrictions = driver.LicenseRestrictions,
                TOP = driver.TOP,
                TypeOfVehicle = driver.TypeOfVehicle,
                Demerit = driver.Demerit,
                Email = driver.Email,
                Remarks = driver.Remarks
            };
            return PartialView("_EditPartial", model);
        }

        // POST: Drivers/Edit
        [HttpPost]
        public IActionResult Edit(DriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                var driver = _context.Drivers.Find(model.Id);
                if (driver == null) return NotFound();

                driver.UserName = model.UserName;
                driver.FirstName = model.FirstName;
                driver.LastName = model.LastName;
                driver.LicenseNumber = model.LicenseNumber;
                driver.Birthdate = model.Birthdate;
                driver.PlateNumber = model.PlateNumber;
                driver.LicenseRestrictions = model.LicenseRestrictions;
                driver.TOP = model.TOP;
                driver.TypeOfVehicle = model.TypeOfVehicle;
                driver.Demerit = model.Demerit;
                driver.Email = model.Email;
                driver.Remarks = model.Remarks;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_CreatePartial", model);
        }

        // GET: Drivers/Details/{id}
        public IActionResult Details(int id)
        {
            var driver = _context.Drivers.Find(id);
            if (driver == null) return NotFound();

            var model = new DriverViewModel
            {
                Id = driver.Id,
                UserName = driver.UserName,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber,
                Birthdate = driver.Birthdate,
                PlateNumber = driver.PlateNumber,
                LicenseRestrictions = driver.LicenseRestrictions,
                TOP = driver.TOP,
                TypeOfVehicle = driver.TypeOfVehicle,
                Demerit = driver.Demerit,
                Email = driver.Email,
                Remarks = driver.Remarks
            };
            return PartialView("_DetailsDriver", model);
        }

        // POST: Drivers/Delete/{id}
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    var driver = _context.Drivers.Find(id);
        //    if (driver != null)
        //    {
        //        _context.Drivers.Remove(driver);
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
