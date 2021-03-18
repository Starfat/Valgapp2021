using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valgapplikasjon;
using Valgapplikasjon.Models;

namespace ValgApp.Controllers
{

    
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize (Roles = "Admin, Kontrollør")]
        public IActionResult Avstemming()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Kontrollør, Bruker")]
        public IActionResult Nominering()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Registrering()
        {
            return View();
        }

        //kan fjernes
        [AllowAnonymous]
        public IActionResult Logginn()
        {
            return View();
        }

        // kan fjernes
        [AllowAnonymous]
        public IActionResult Registrerbruker()
        {
            return View();
        }
    }
}
