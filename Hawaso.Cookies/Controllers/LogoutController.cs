using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Hawaso.Cookies.Controllers
{
    public class LogoutController : Controller
    {
        //public async Task<IActionResult> Index()
        //{
        //    // [!] 인증 해제: 쿠키를 삭제하여 인증 정보를 제거
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return LocalRedirect("/");
        //}

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            // [!] 인증 해제: 쿠키를 삭제하여 인증 정보를 제거
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
