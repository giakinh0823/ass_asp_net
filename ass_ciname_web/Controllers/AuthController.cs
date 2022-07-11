using ass_ciname_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ass_ciname_web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var conf = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                string username = conf["User:username"];
                string password = conf["User:username"];
                if (user.Username.Equals(username) && user.Password.Equals(password))
                {
                    HttpContext.Session.SetString("user", user.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Dont have that user!";
                }
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Auth");
        }
    }
}
