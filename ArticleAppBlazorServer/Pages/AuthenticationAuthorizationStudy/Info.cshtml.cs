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
            // ClaimsPrincipal���� ���� ����
            UserName = User.Identity?.Name ?? "�� �� ����";
            UserRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "����";
        }
    }
}
