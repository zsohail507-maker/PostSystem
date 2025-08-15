using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using postSystem.Models;
using postSystem.Models.Data;
using postSystem.Models.Entities;

namespace postSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly MasterDBContext _masterDBContext;
        public LoginController(MasterDBContext context) {
            _masterDBContext = context;        
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserData user)
        {
            Users? oldRecord = _masterDBContext.Users.FirstOrDefault(u => u.Email == user.Email);
            
            if (user.Name == null)
            {
                ModelState.AddModelError(nameof(user.Name), "Name cannot be null");
                return View(user);
            }
            if (user.Email == null)
            {
                ModelState.AddModelError(nameof(user.Email), "Email cannot be null");
                return View(user);
            }
            if (user.Password == null)
            {
                ModelState.AddModelError(nameof(user.Password), "Password cannot be null");
                return View(user);
            }

            if (oldRecord != null)
            {
                ModelState.AddModelError(nameof(user.Email), "Email already in use.");
                return View(user);
            }


            Users record = new Users();
            record.Name = user.Name;
            record.Password = user.Password;
            record.Email = user.Email;

            _masterDBContext.Users.Add(record);
            _masterDBContext.SaveChanges();

            return View("Index");
        }

        // Login page
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserCredential user)
        {
            if (user.Email == null || user.Password == null)
            {
                ModelState.AddModelError(nameof(user.Password), "Please fill all the fields.");
            }

            Users? oldrecord =  _masterDBContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (oldrecord == null) 
            {
                ModelState.AddModelError(nameof(user.Email), "Record Not Found");
                return View(user);
            }

            if (oldrecord.Password != user.Password)
            {
                ModelState.AddModelError(nameof(user.Password), "Incorrect Password");
                return View(user);
            }

            HttpContext.Session.SetString("email", user.Email);

            return RedirectToAction("Dashboard", "Post");
        }
    }
}
