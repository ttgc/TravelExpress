using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Travel_Express.Models;

namespace Travel_Express.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        

        /*[Route("/Home/ConfirmRegister", Name = "RegisterTravel")]
        //public ActionResult Register([Bind("ID,Title,ReleaseDate,Genre,Price")] Travel_Express.Models.))
        public IActionResult Register()
        {
            System.Diagnostics.Debug.WriteLine("Some Log");
            return ConfirmNewTravel();
        }

        public IActionResult ConfirmNewTravel()
        {
            System.Diagnostics.Debug.WriteLine("Some Log");
            return View();
        }*/
        
        [HttpPost]
        public IActionResult Test( [Bind("dummy")] Travel_Express.Models.SimpleClassModel simpleInstance)
        {
            System.Diagnostics.Debug.WriteLine("Message: "+simpleInstance.dummy+", full:"+simpleInstance);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
