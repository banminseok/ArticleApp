using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticleAppBlazorServer.Pages.AuthenticationAuthorizationStudy
{
    public class IndexModel : PageModel
    {

        public string LoginMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            // ����ڰ� �����Ǿ����� Ȯ��
            if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                // ����� ������ ����մϴ�.
                LoginMessage = "<h1>����� ����</h1>";
                LoginMessage += $"<p>IsAuthenticated: {HttpContext.User.Identity.IsAuthenticated}</p>";
                LoginMessage += $"<p>Name: {HttpContext.User.Identity.Name}</p>";
            }
            else
            {
                LoginMessage += "<h3>�α������� �ʾҽ��ϴ�.</h3>";
            }
        }
    }
}
