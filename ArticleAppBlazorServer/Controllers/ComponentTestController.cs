using Microsoft.AspNetCore.Mvc;

namespace ArticleAppBlazorServer.Controllers
{
    public class ComponentTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
