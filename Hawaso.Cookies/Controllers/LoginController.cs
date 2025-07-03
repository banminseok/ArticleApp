using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hawaso.Cookies.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            if(email == "a@a.com" && password=="password")
            {
                //[!] 인증 부여: 인증된 사용자의 주요 정보(Name, Role...)를 기록
                var claims = new List<Claim>
                {
                    //로그인 아이디 지정
                        //new Claim("Email",email),
                    new Claim(ClaimTypes.Email, email),
                        //new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Name, "Administrator"),
                    // 기본 역할 지정 "Role" 기능에 "Users" 역할 지정
                    new Claim(ClaimTypes.Role, "Users") //추가 정보기록
                };

                var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authenticationProperties = new AuthenticationProperties()
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60), //쿠키 만료시간
                    IssuedUtc = DateTimeOffset.UtcNow, //쿠키 발급시간
                    IsPersistent = true, //쿠키 유지 여부
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, //쿠키 인증 스킴
                    new ClaimsPrincipal(ci), //ClaimsPrincipal 객체
                    authenticationProperties //인증 속성
                );

                return LocalRedirect(Url.Content("~/"));
            }
            return View();
        }
    }
}
