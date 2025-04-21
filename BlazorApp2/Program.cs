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
    .AddCookie(); // ��Ű ���� ��Ű�� �߰�
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
// ��������Ʈ �� ���Ʈ ����
//app.MapGet("/", () => "Hello World!");
app.MapGet("/ban", async context =>
{
    // �޴� HTML ���ڿ��� �����Ͽ� ����
    string content = "<h1>ASP.NET Core ������ ���� �ʰ��� �ڵ�</h1>";
    content += "<a href=\"/banLogin\">�α���</a><br />";
    content += "<a href=\"/Login/User\">�α���(User)</a><br />";
    content += "<a href=\"/Login/Administrator\">�α���(Administrator)</a><br />";
    content += "<a href=\"/banInfo\">����</a><br />";
    content += "<a href=\"/InfoDetails\">����(Details)</a><br />";
    content += "<a href=\"/InfoJson\">����(JSON)</a><br />";
    content += "<a href=\"/Logout\">�α׾ƿ�</a><br />";
    content += "<hr /><a href=\"/Landing\">����������</a><br />";
    content += "<a href=\"/Greeting\">ȯ��������</a><br />";
    content += "<a href=\"/Dashboard\">����������</a><br />";
    content += "<a href=\"/api/AuthService\">�α��� ����(JSON)</a><br />";


    // ���� ��� ����
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    await context.Response.WriteAsync(content);
});
#endregion

#region 1. Login
// "/Login" ��η� GET ��û�� ó���մϴ�.
app.MapGet("/banLogin", async context =>
{
    // Ŭ����(Claim) ����Ʈ�� �����ϰ� ����� ���̵� Ŭ������ �߰��մϴ�.
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "userID") // ����� �̸�(���̵�) Ŭ����
    };

    // ��Ű ���� ��Ű���� ����Ͽ� Ŭ���� ���̵�ƼƼ�� �����մϴ�.
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //.AspNetCore.Cookies

    // Ŭ���� ���̵�ƼƼ�� ����Ͽ� Ŭ���� ���������� �����մϴ�.
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    // ����ڸ� �α��� ó���ϰ� ��Ű ���� ��Ű���� ���� �����մϴ�.
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

    // ���� ����� "Content-Type"�� �����Ͽ� �ѱ� ���ڰ� �ùٸ��� ǥ�õǵ��� �մϴ�.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    // Ŭ���̾�Ʈ�� "�α��� �Ϸ�" �޽����� ����մϴ�.
    await context.Response.WriteAsync("<h3>�α��� �Ϸ�</h3>");
});
#endregion

#region 2. Info
// "/Info" ��η� GET ��û�� ó���մϴ�.
app.MapGet("/banInfo", async context =>
{
    string result = "";
    // ����ڰ� �����Ǿ����� Ȯ��
    if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
    {
        // ����� ������ ����մϴ�.
        result = "<h1>����� ����</h1>";
        result += $"<p>IsAuthenticated: {context.User.Identity.IsAuthenticated}</p>";
        result += $"<p>Name: {context.User.Identity.Name}</p>";
    }
    else
    {
        result += "<h3>�α������� �ʾҽ��ϴ�.</h3>";
    }

    // ���� ����� "Content-Type"�� �����Ͽ� �ѱ� ���ڰ� �ùٸ��� ǥ�õǵ��� �մϴ�.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";

    // ����� ������ ����մϴ�.
    await context.Response.WriteAsync(result, Encoding.Default);   //System.Text.Encoding.Default
});
#endregion
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
