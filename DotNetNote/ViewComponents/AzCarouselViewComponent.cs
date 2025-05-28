using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.ViewComponents
{
    public class AzCarouselViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // ViewBag.CopyRight = $"Copyright © {DateTime.Now.Year}. All rights reserved.";
            // return View("Default");
            return View();
        }
    }
}
