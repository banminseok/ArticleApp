using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    public class ViewBagDemoController : Controller
    {
        /// <summary>
        /// ViewBag 개체로 컨트롤러에서 뷰로 데이터 전달
        /// </summary>
        public IActionResult Index()
        {
            ViewBag.Name = "박용준";
            ViewBag.Age = 21;
            ViewBag.원하는모든단어 = "모든 값...";

            // ViewBag.Address와 ViewData["Address"]는 동일 표현 
            //ViewBag.Address = "세종시...";
            ViewData["Address"] = "세종시...";

            return View();
        }

        public IActionResult JavaScriptStringTest()
        {
            ViewBag.AlertMessage = "안녕하세요.";
            return View();
        }

        public IActionResult ViewWithModel()
        {
            // ViewData["Id"] = "1";
            // ViewData["Name"] = "박용준";
            var model = new DemoModel
            {
                Id = "1",
                Name = "박용준"
            };
            return View(model);
        }


        public IActionResult ViewWithListOfDemo()
        {
            List<DemoModel> models = new List<DemoModel>()
            {
                new DemoModel{ Id = "1", Name = "박용준" },
                new DemoModel{ Id = "2", Name = "구충현" },
                new DemoModel{ Id = "3", Name = "박준" },
                new DemoModel{ Id = "4", Name = "박용" },
                new DemoModel{ Id = "5", Name = "용준" },
            };
            return View(models);
        }
    }


    public class DemoModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
