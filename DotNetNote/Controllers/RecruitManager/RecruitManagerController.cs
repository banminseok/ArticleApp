using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers.RecruitManager
{
    public class RecruitManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
