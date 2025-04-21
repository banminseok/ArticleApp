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

        // Razor 페이지에 전달할 속성
        public string WelcomeMessage { get; set; } = string.Empty;

        // 로그인 페이지를 GET 요청으로 호출할 때 실행되는 메서드
        public async Task<IActionResult> OnGet()
        {
            // 클레임(Claim) 리스트를 생성하고 사용자 아이디 클레임을 추가합니다.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "userID"), // 사용자 이름(아이디) 클레임
                new Claim(ClaimTypes.Role, "Admin")
            };
            // 쿠키 인증 스키마를 사용하여 클레임 아이덴티티를 생성합니다.
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //.AspNetCore.Cookies

            // 클레임 아이덴티티를 사용하여 클레임 프린시펄을 생성합니다.
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // 사용자를 로그인 처리하고 쿠키 인증 스키마를 통해 인증합니다.
            // HttpContext를 통해 로그인 처리
            // 로그인 처리
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Razor 페이지에 전달할 값 설정
            WelcomeMessage = "Welcome to the Login Page!";
            // 로그인 후 리다이렉트할 페이지로 이동합니다.
            //return RedirectToPage("/AuthenticationAuthorizationStudy/Index");
            if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                // 사용자 정보를 출력합니다.
                WelcomeMessage = "<h1>사용자 정보</h1>";
                WelcomeMessage += $"<p>IsAuthenticated: {HttpContext.User.Identity.IsAuthenticated}</p>";
                WelcomeMessage += $"<p>Name: {HttpContext.User.Identity.Name}</p>";
            }
            else
            {
                WelcomeMessage += "<h3>로그인하지 않았습니다.</h3>";
            }

            return Page(); // 사용자 없으면 현재 페이지 유지
        }
    }
}
