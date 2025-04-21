using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ArticleAppBlazorServer.Pages.AuthenticationAuthorizationStudy
{
    public class InfoModel : PageModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }

        public void OnGet()
        {
            // ClaimsPrincipal에서 정보 추출
            UserName = User.Identity?.Name ?? "알 수 없음";
            UserRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "없음";
        }
    }
}
