using Microsoft.AspNetCore.Mvc;

namespace ArticleAppBlazorServer.Controllers
{
    public class AppSettingsDemoController : Controller
    {
        private readonly IConfiguration _configuration;

        public AppSettingsDemoController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            //https://www.youtube.com/watch?v=310v9Gk0om8&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=37
            //appsettings 파일의 데이터를 IConfiguration 개체로 읽어오기 및 secrets 파일로 보안된 설정값 관리하기
            string con1 = _configuration.GetSection("StorageConnectionString1").Value;   //==>Storage String 1
            string con2 = _configuration.GetSection("BlobStroageConnectionString").GetSection("Site1").Value;

            string site2 = _configuration.GetValue<string>("BlobStroageConnectionString:Site2");
            return Content($"{con1},{con2},{site2}");
        }
    }
}
