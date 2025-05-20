using Dul.Web;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    public class PartialViewDemoController : Controller
    {
        public IActionResult Index()
        {
            var pager = new PagerBase
            {
                PageIndex = 1,
                PageCount = 3,
                PageSize = 10,
                RecordCount = 100,
                Url = "PartialViewDemo",
            };
            ViewBag.PageModel = pager;
            return View();
        }
    }
}
