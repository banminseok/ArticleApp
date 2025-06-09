using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers.Board
{
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
