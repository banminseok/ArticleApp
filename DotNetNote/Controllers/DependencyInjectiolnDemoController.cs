using DotNetNote.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    public class DependencyInjectiolnDemoController : Controller
    {
        private readonly CopyrightService _svc2;
        private readonly CopyrightService _svc3;
        private readonly ICopyrightService _svc;

        public DependencyInjectiolnDemoController(ICopyrightService service, CopyrightService service3)
        {
            _svc2 = new CopyrightService();
            _svc = service;
            _svc3 = service3;
        }
        public IActionResult Index()
        {
            //CopyrightService _svc = new CopyrightService();
            ViewBag.CopyRight = _svc.GetCopyrightString();
            //ViewBag.CopyRight = $"Copyright © {DateTime.Now.Year}. All rights reserved.";
            return View();
        }

        public IActionResult About()
        {
            //CopyrightService _svc = new CopyrightService();
            ViewBag.CopyRight = _svc.GetCopyrightString();
            //ViewBag.CopyRight = $"Copyright © {DateTime.Now.Year}. All rights reserved.";
            return View();
        }
    }
}
