using Microsoft.AspNetCore.Mvc;

namespace postSystem.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult About()
        {
            ViewData["Title"] = "About Us";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact Us";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy Policy";
            return View();
        }

        public IActionResult Terms()
        {
            ViewData["Title"] = "Terms and Conditions";
            return View();
        }

        public IActionResult Disclaimer()
        {
            ViewData["Title"] = "Disclaimer";
            return View();
        }
    }
}
