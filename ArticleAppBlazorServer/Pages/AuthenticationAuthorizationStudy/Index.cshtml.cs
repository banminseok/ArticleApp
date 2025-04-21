using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticleAppBlazorServer.Pages.AuthenticationAuthorizationStudy
{
    public class IndexModel : PageModel
    {

        public string LoginMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            // 사용자가 인증되었는지 확인
            if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                // 사용자 정보를 출력합니다.
                LoginMessage = "<h1>사용자 정보</h1>";
                LoginMessage += $"<p>IsAuthenticated: {HttpContext.User.Identity.IsAuthenticated}</p>";
                LoginMessage += $"<p>Name: {HttpContext.User.Identity.Name}</p>";
            }
            else
            {
                LoginMessage += "<h3>로그인하지 않았습니다.</h3>";
            }
        }
    }
}
