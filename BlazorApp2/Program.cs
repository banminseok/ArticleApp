using BlazorApp2.Areas.Identity;
using BlazorApp2.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // 쿠키 인증 스키마 추가
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#region 0. Menu
// 엔드포인트 및 라우트 설정
//app.MapGet("/", () => "Hello World!");
app.MapGet("/ban", async context =>
{
    // 메뉴 HTML 문자열을 생성하여 응답
    string content = "<h1>ASP.NET Core 인증과 권한 초간단 코드</h1>";
    content += "<a href=\"/banLogin\">로그인</a><br />";
    content += "<a href=\"/Login/User\">로그인(User)</a><br />";
    content += "<a href=\"/Login/Administrator\">로그인(Administrator)</a><br />";
    content += "<a href=\"/banInfo\">정보</a><br />";
    content += "<a href=\"/InfoDetails\">정보(Details)</a><br />";
    content += "<a href=\"/InfoJson\">정보(JSON)</a><br />";
    content += "<a href=\"/Logout\">로그아웃</a><br />";
    content += "<hr /><a href=\"/Landing\">랜딩페이지</a><br />";
    content += "<a href=\"/Greeting\">환영페이지</a><br />";
    content += "<a href=\"/Dashboard\">관리페이지</a><br />";
    content += "<a href=\"/api/AuthService\">로그인 정보(JSON)</a><br />";


    // 응답 헤더 설정
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    await context.Response.WriteAsync(content);
});
#endregion

#region 1. Login
// "/Login" 경로로 GET 요청을 처리합니다.
app.MapGet("/banLogin", async context =>
{
    // 클레임(Claim) 리스트를 생성하고 사용자 아이디 클레임을 추가합니다.
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "userID") // 사용자 이름(아이디) 클레임
    };

    // 쿠키 인증 스키마를 사용하여 클레임 아이덴티티를 생성합니다.
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //.AspNetCore.Cookies

    // 클레임 아이덴티티를 사용하여 클레임 프린시펄을 생성합니다.
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    // 사용자를 로그인 처리하고 쿠키 인증 스키마를 통해 인증합니다.
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

    // 응답 헤더의 "Content-Type"을 설정하여 한글 문자가 올바르게 표시되도록 합니다.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    // 클라이언트에 "로그인 완료" 메시지를 출력합니다.
    await context.Response.WriteAsync("<h3>로그인 완료</h3>");
});
#endregion

#region 2. Info
// "/Info" 경로로 GET 요청을 처리합니다.
app.MapGet("/banInfo", async context =>
{
    string result = "";
    // 사용자가 인증되었는지 확인
    if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
    {
        // 사용자 정보를 출력합니다.
        result = "<h1>사용자 정보</h1>";
        result += $"<p>IsAuthenticated: {context.User.Identity.IsAuthenticated}</p>";
        result += $"<p>Name: {context.User.Identity.Name}</p>";
    }
    else
    {
        result += "<h3>로그인하지 않았습니다.</h3>";
    }

    // 응답 헤더의 "Content-Type"을 설정하여 한글 문자가 올바르게 표시되도록 합니다.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";

    // 사용자 정보를 출력합니다.
    await context.Response.WriteAsync(result, Encoding.Default);   //System.Text.Encoding.Default
});
#endregion
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
