using ArticleAppBlazorServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleAppBlazorServer.Controllers
{
    public class SingletonDemoController : Controller
    {
        //[1] 생성자에 클래스 주입
        //private readonly InfoService _svc;

        //public SingletonDemoController(InfoService svc)
        //{
        //    _svc = svc;
        //}

        // [2] 생성자에 인터페이스 주입
        private readonly IInfoService _svc;

        public SingletonDemoController(IInfoService svc)
        {
            _svc = svc;
        }

        public IActionResult Index()
        {
            ViewData["Url"] = "m.stmnet.co.kr";
            return View();
        }
        public IActionResult ConstructorInjectionDemo()
        {
            
            string url = _svc.GetUrl();
            ViewData["Url"] = url;
            return View("Index");
        }
        public IActionResult InfoServiceDemo()
        {
            InfoService svc = new InfoService();
            string url = svc.GetUrl();
            ViewData["Url"] = url;
            return View("Index");
        }
    }
}
