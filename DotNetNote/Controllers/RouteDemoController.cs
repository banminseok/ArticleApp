using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    // Route 특성을 이용한 어크리뷰트 라우팅
    [Route("api/RouteDemo")]
    public class RouteDemoController : Controller
    {
        [Route(""), Route("Index"), Route("Index2")]
        public string Index()
        {
            return "어트리뷰트 라우팅";
        }
    }
}
