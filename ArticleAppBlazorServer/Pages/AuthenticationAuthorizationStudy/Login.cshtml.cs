using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
using System.Security.Claims;

namespace ArticleAppBlazorServer.Pages.AuthenticationAuthorizationStudy
{
    public class LoginModel : PageModel
    {

        // Razor �������� ������ �Ӽ�
        public string WelcomeMessage { get; set; } = string.Empty;

        // �α��� �������� GET ��û���� ȣ���� �� ����Ǵ� �޼���
        public async Task<IActionResult> OnGet()
        {
            // Ŭ����(Claim) ����Ʈ�� �����ϰ� ����� ���̵� Ŭ������ �߰��մϴ�.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "userID"), // ����� �̸�(���̵�) Ŭ����
                new Claim(ClaimTypes.Role, "Admin")
            };
            // ��Ű ���� ��Ű���� ����Ͽ� Ŭ���� ���̵�ƼƼ�� �����մϴ�.
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //.AspNetCore.Cookies

            // Ŭ���� ���̵�ƼƼ�� ����Ͽ� Ŭ���� ���������� �����մϴ�.
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // ����ڸ� �α��� ó���ϰ� ��Ű ���� ��Ű���� ���� �����մϴ�.
            // HttpContext�� ���� �α��� ó��
            // �α��� ó��
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Razor �������� ������ �� ����
            WelcomeMessage = "Welcome to the Login Page!";
            // �α��� �� �����̷�Ʈ�� �������� �̵��մϴ�.
            //return RedirectToPage("/AuthenticationAuthorizationStudy/Index");
            if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                // ����� ������ ����մϴ�.
                WelcomeMessage = "<h1>����� ����</h1>";
                WelcomeMessage += $"<p>IsAuthenticated: {HttpContext.User.Identity.IsAuthenticated}</p>";
                WelcomeMessage += $"<p>Name: {HttpContext.User.Identity.Name}</p>";
            }
            else
            {
                WelcomeMessage += "<h3>�α������� �ʾҽ��ϴ�.</h3>";
            }

            return Page(); // ����� ������ ���� ������ ����
        }
    }
}
