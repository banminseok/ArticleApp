using ArticleApp.Models;
using ArticleAppBlazorServer.Areas.Identity;
using ArticleAppBlazorServer.Areas.Identity.Models;
using ArticleAppBlazorServer.Areas.Identity.Services;
using ArticleAppBlazorServer.Data;
using ArticleAppBlazorServer.Managers;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text;
using UploadApp.Shared;

var builder = WebApplication.CreateBuilder(args);
// [1] Startup.ConfigureServices ���� (�� startup.cs ����)
// ���� �߰�
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//���ο� DbContext�� �߰�
#region ���ο� DbContext �߰� - CandidateAppDbContext
// ���ο� DbContext �߰� 

//[a] MVC, RazorPages, Web API������ DbContext ��� ����
//builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

//[b] Blazor Server������ DbContextFactory ��� ����
// mvc �������� Blazor ������Ʈ�� 2�� �̻� ����ϱ� ���ؼ��� DbContextFactory�� ����ؾ� �Ѵ�.
// �ϳ��� Blazor �������� DbContext�� 2�� �̻� ����ϸ�, Blazor������Ʈ�� 2���̻� ����ϸ�  ������ �߻��Ѵ�.
// // https://docs.microsoft.com/ko-kr/aspnet/core/blazor/data/ef-core?view=aspnetcore-6.0#use-a-dbcontext-factory
//  https://www.youtube.com/watch?v=HTic5S5Z6iA&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=26
// https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso/Components/Pages/VendorPages/Models/07_VendorRepositoryPermanentDelete.cs#L9
builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

# endregion

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // ��Ű ���� ��Ű�� �߰�

/*.AddCookie(options =>
{
    options.LoginPath = "/Login"; // �α��� ������ ���
    options.AccessDeniedPath = "/AccessDenied"; // ���� �ź� ������ ���
});*/



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

// �̰� ������  ��Ű ���� �� ����� ���ܼ� �ϴ� �ּ�ó�� �صд�.
//builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();  // MVC ��Ʈ�ѷ� ���
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // ������ ��忡�� �� ���� ǥ��
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

#region ��������(NoticeApp) ���� ������(���Ӽ�) ���� ���� �ڵ常 ���� ��Ƽ� ���� 
// ArticleAppDbContext.cs Inject: New DbContext Add
//builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
builder.Services.AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient); // ���� �����ֱ�: Transient�� �ߺ� ȣ��� ���..
//builder.Services.AddDbContextFactory<ArticleAppDbContext>(options => options.UseSqlServer(connectionString),
//    ServiceLifetime.Scoped  // ����� ������ ����
//builder.Services.AddDbContextFactory<IdeaAppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<IdeaAppDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddScoped<IdeaAppDbContext>(provider =>
//{
//    var factory = provider.GetRequiredService<IDbContextFactory<IdeaAppDbContext>>();
//    return factory.CreateDbContext();
//}
//);


// IArticleRepository.cs Inject: DI Container�� ����(�������丮) ���
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<INoticeRepositoryAsync, NoticeRepositoryAsync>();
builder.Services.AddTransient<IUploadRepository, UploadRepository>();
#endregion

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddTransient<IFileStorageManager, UploadAppFileStorageManager>();  //Local File System
//builder.Services.AddTransient<IFileStorageManager, BlobStoragedManager>();  //Azure Blob Storage
builder.Services.AddTransient<IReplyRepository, ReplyRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepositoryInMemory>();
builder.Services.AddSingleton<IInfoService, InfoService>();
builder.Services.AddTransient<IIdeaRepository, IdeaRepository>();

// DI Container�� ���� ���
builder.Services.AddTransient<IVideoRepository, VideoRepositoryEfCoreAsync>();
//builder.Services.AddSingleton<IVideoRepositoryAsync>(new VideoRepositoryAdoNetAsync(connectionString));
//builder.Services.AddSingleton<IVideoRepositoryAsync>(new VideoRepositoryDapperAsync(connectionString));

builder.Services.AddTransient<IMediaRepository, MediaRepository>();
builder.Services.AddTransient<IMachineRepository, MachineRepository>();

//���Ӽ����� �߰�
builder.Services.AddTransient<IEmailSender, EmailSender>(); // EmailSender.cs
builder.Services.AddScoped<HttpClient>(); // MatBlazor �ʼ� ����

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

#region Serilog
//// 31.8.4. Serilog�� ����Ͽ� �α� ���� ����ϱ� 
//// ILoggerFactory loggerFactory
Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Debug()
    //.WriteTo.File(Path.Combine(app.Environment.ContentRootPath, "DnsLogs.txt"), rollingInterval: RollingInterval.Day)
    //.WriteTo.File("DnsLogs.txt", rollingInterval: RollingInterval.Day)
    //.ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Information("�ءء�[!] Blazor Server Application started.");
// Serilog�� DI �����̳ʿ� �߰�
//builder.Host.UseSerilog();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
#endregion

var app = builder.Build();

// [2] Startup.Configure ���� 
// ���� ȯ�� ����
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    //����ȯ�濡�� �����Ͱ� ���ٸ� �����͸� �־��
    //04_06_CandidatesSeedData_Candidates ���̺� �⺻ ������ �Է�
    //https://www.youtube.com/watch?v=b7Ft2qRaEHU&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=17
    CandidateSeedData(app);

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//�̵���� ����
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


//app.MapControllers();
app.MapDefaultControllerRoute();  //MVC ��Ʈ�ѷ� �����
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


#region CandidateSeedData: Candidates ���̺� �⺻ ������ �Է�
// Candidates ���̺� �⺻ ������ �Է�
static void CandidateSeedData(WebApplication app)
{
    // https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/dependency-injection
    // ?view=aspnetcore-6.0#resolve-a-service-at-app-start-up
    using (var serviceScope = app.Services.CreateScope())
    {
        var services = serviceScope.ServiceProvider;

        var candidateDbContext = services.GetRequiredService<CandidateAppDbContext>();

        // Candidates ���̺� �����Ͱ� ���� ������ ������ �Է�
        if (!candidateDbContext.Candidates.Any())
        {
            candidateDbContext.Candidates.Add(
                new Candidate { FirstName = "�浿", LastName = "ȫ", IsEnrollment = false });
            candidateDbContext.Candidates.Add(
                new Candidate { FirstName = "�λ�", LastName = "��", IsEnrollment = false });

            candidateDbContext.SaveChanges();
        }
    }
}
#endregion


