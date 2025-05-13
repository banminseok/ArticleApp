using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test.WebMVC.Models;

namespace Test.WebMVC.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;

        public AboutController(ILogger<AboutController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}
