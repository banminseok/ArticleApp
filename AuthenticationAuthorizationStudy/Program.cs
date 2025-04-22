using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

/// <summary>
/// https://github.com/VisualAcademy/AuthenticationAuthorization/blob/main/AuthenticationAuthorization/VisualAcademy/Program.cs
/// </summary>



var builder = WebApplication.CreateBuilder(args);

// [1] Startup.ConfigureServices ���� (�� startup.cs ����)
// ���� �߰�
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // ��Ű ���� ��Ű�� �߰�

/*.AddCookie(options =>
{
    options.LoginPath = "/Login"; // �α��� ������ ���
    options.AccessDeniedPath = "/AccessDenied"; // ���� �ź� ������ ���
});*/

var app = builder.Build();

// [2] Startup.Configure ���� 
// ���� ȯ�� ����
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//�̵���� ����
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); // [Authorize] Ư�� ���

#region 0. Menu
// ��������Ʈ �� ���Ʈ ����
//app.MapGet("/", () => "Hello World!");
app.MapGet("/", async context =>
{
    // �޴� HTML ���ڿ��� �����Ͽ� ����
    string content = "<h1>ASP.NET Core ������ ���� �ʰ��� �ڵ�</h1>";
    content += "<a href=\"/Login\">�α���</a><br />";
    content += "<a href=\"/Login/User\">�α���(User)</a><br />";
    content += "<a href=\"/Login/Administrator\">�α���(Administrator)</a><br />";
    content += "<a href=\"/Info\">����</a><br />";
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
app.MapGet("/Login", async context =>
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
app.MapGet("/Info", async context =>
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

#region 3. InfoJson
// "/Info" ��η� GET ��û�� ó���մϴ�.
app.MapGet("/InfoJson", async context =>
{
    string json = "";
    // ����ڰ� �����Ǿ����� Ȯ��
    if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
    {
        // ����� Ŭ���� ������ JSON���� ����ȭ
        var claims = context.User.Claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value });
        json += JsonSerializer.Serialize<IEnumerable<ClaimDto>>(
            claims,
            new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }); // �ѱ� ���ڵ� ���� �ѱ۷� �״�� ���.
    }
    else
    {
        json += "{}";
    }

    // MIME Ÿ���� JSON �������� ����
    // ���� ����� "Content-Type"�� �����Ͽ� �ѱ� ���ڰ� �ùٸ��� ǥ�õǵ��� �մϴ�.
    context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";

    // ����� ������ ����մϴ�.
    await context.Response.WriteAsync(json);
});
#endregion


#region 4. Logout
// "/Logout" ��η� GET ��û�� ó���մϴ�.
app.MapGet("/Logout", async context =>
{
    // ����ڸ� �α׾ƿ� ó���ϰ� ��Ű ���� ��Ű���� ���� �α׾ƿ��մϴ�.
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    // ���� ����� "Content-Type"�� �����Ͽ� �ѱ� ���ڰ� �ùٸ��� ǥ�õǵ��� �մϴ�.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    // Ŭ���̾�Ʈ�� "�α׾ƿ� �Ϸ�" �޽����� ����մϴ�.
    await context.Response.WriteAsync("<h3>�α׾ƿ� �Ϸ�</h3>");
});
#endregion


#region 5. Login/{Username}
// "/Login/{Username}" ��η� GET ��û�� ó���մϴ�.  "���Ʈ �Ķ����" �б�
app.MapGet("/Login/{Username}", async context =>
{
    // URL���� ����� �̸��� �����ɴϴ�. ("���Ʈ �Ķ����" �б�)
    var username = context.Request.RouteValues["Username"]?.ToString() ?? string.Empty;

    // Ŭ����(Claim) ����Ʈ�� �����ϰ� ����� ���̵� Ŭ������ �߰��մϴ�.
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, username), // ����� �̸�(���̵�) Ŭ����
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Email, username.ToLower()+"@youremail.com"),
        new Claim(ClaimTypes.Role, "Users"), // ����(Role) Ŭ����, �⺻������ "Users" ������ �������� ����
        //new Claim(ClaimTypes.Role, "Administrators"), // ����(Role) Ŭ����, �⺻������ "Admin" ������ �������� ����
        new Claim("���ϴ� �̸�", "���ϴ� ��")
    };
    // ���� ����� �̸��� "Administrator"��� "Administrators" ������ �߰��մϴ�.
    if (username == "Administrator")
    {
        claims.Add(new Claim(ClaimTypes.Role, "Administrators"));
    }
    // ��Ű ���� ��Ű���� ����Ͽ� Ŭ���� ���̵�ƼƼ�� �����մϴ�.
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //.AspNetCore.Cookies
    // Ŭ���� ���̵�ƼƼ�� ����Ͽ� Ŭ���� ���������� �����մϴ�.
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    // ����ڸ� �α��� ó���ϰ� ��Ű ���� ��Ű���� ���� �����մϴ�.
    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        claimsPrincipal,
        new AuthenticationProperties { IsPersistent = true }  //�������� �ݾƵ� �α��� ���� (�⺻���� false)
    );
    // ���� ����� "Content-Type"�� �����Ͽ� �ѱ� ���ڰ� �ùٸ��� ǥ�õǵ��� �մϴ�.
    context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
    // Ŭ���̾�Ʈ�� "�α��� �Ϸ�" �޽����� ����մϴ�.
    await context.Response.WriteAsync("<h3>�α��� �Ϸ�</h3>");
});
#endregion


#region 6. InfoDetails
// "/InfoDetails" ��η� GET ��û�� ó���մϴ�.
app.MapGet("/InfoDetails", async context =>
{
    string result = "";
    // ����ڰ� �����Ǿ����� Ȯ��
    if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
    {
        // ����� ������ ����մϴ�.
        result = "<h1>����� ����</h1>";
        result += $"<p>IsAuthenticated: {context.User.Identity.IsAuthenticated}</p>";
        result += $"<p>Name: {context.User.Identity.Name}</p>";
        result += "<h2>Ŭ���� ����</h2>";
        foreach (var claim in context.User.Claims)
        {
            result += $"<p>{claim.Type}: {claim.Value}</p>";
        }

        // ����ڰ� "Administrators"�� "Users" ������ ��� ������ �ִ��� Ȯ��
        if (context.User.IsInRole("Administrators") && context.User.IsInRole("Users"))
        {
            result += "<h3>������(Administrators)�� �����(Users) ������ ��� ������ �ֽ��ϴ�.</h3>";
        }
        else if (context.User.IsInRole("Administrators"))
        {
            result += "<h3>������(Administrators) ������ ������ �ֽ��ϴ�.</h3>";
        }
        else if (context.User.IsInRole("Users"))
        {
            result += "<h3>�����(Users) ������ ������ �ֽ��ϴ�.</h3>";
        }
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


// ��Ʈ�ѷ� ����
//app.MapControllers(); // [7] MVC Controller 
app.MapDefaultControllerRoute();  //[Route("/Landing")] �̷������� ������� �������ָ� �̰� ���� �ʾƵ� ��
app.Run();



#region DTO

/// <summary>
/// ����� ���� DTO Ŭ���� Data Transfer Object Class
/// </summary>
public class ClaimDto
{
    [Required]
    public string? Type { get; set; }  //prop tab tab
    [Required]
    public string? Value { get; set; }
}
#endregion


#region 7. MVC Controller
// MVC ��Ʈ�ѷ� ����
[AllowAnonymous]  //�̰� ������ [�� �޼��忡] Authorize Ư���� �־ ���õ�   
[Route("/Landing")]
public class LandingController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Content("Landing Page [������ ���� ����]");
    }

    [Authorize]
    [HttpGet("/Greeting")]
    public IActionResult Greeting()
    {
        var roleName = HttpContext.User.IsInRole("Administrators") ? "������" : "�����";
        return Content($"<em>{roleName}</em> ��, �ݰ����ϴ�.", "text/html", Encoding.Default);
    }

}

[Authorize(Roles = "Administrators")]
[Route("/Dashboard")]
public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Index() => Content("������ ��, �ݰ����ϴ�.");
}
#endregion

#region 8.Web API Controller
// Web API ��Ʈ�ѷ� ����
[ApiController]
[Route("/api/[controller]")]
public class AuthServiceController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IEnumerable<ClaimDto> Get()
    {
        var claims = HttpContext.User.Claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value });
        return claims;
    }
}
#endregion