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
// [1] Startup.ConfigureServices 영역 (구 startup.cs 파일)
// 서비스 추가
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//새로운 DbContext를 추가
#region 새로운 DbContext 추가 - CandidateAppDbContext
// 새로운 DbContext 추가 

//[a] MVC, RazorPages, Web API에서는 DbContext 사용 가능
//builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

//[b] Blazor Server에서는 DbContextFactory 사용 권장
// mvc 페이지에 Blazor 컴포넌트를 2개 이상 사용하기 위해서는 DbContextFactory를 사용해야 한다.
// 하나의 Blazor 페이지에 DbContext를 2개 이상 사용하면, Blazor컴포넌트를 2개이상 사용하면  오류가 발생한다.
// // https://docs.microsoft.com/ko-kr/aspnet/core/blazor/data/ef-core?view=aspnetcore-6.0#use-a-dbcontext-factory
//  https://www.youtube.com/watch?v=HTic5S5Z6iA&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=26
// https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso/Components/Pages/VendorPages/Models/07_VendorRepositoryPermanentDelete.cs#L9
builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));

# endregion

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // 쿠키 인증 스키마 추가

/*.AddCookie(options =>
{
    options.LoginPath = "/Login"; // 로그인 페이지 경로
    options.AccessDeniedPath = "/AccessDenied"; // 접근 거부 페이지 경로
});*/



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

// 이게 있으면  쿠키 인증 과 충둘이 생겨서 일단 주석처리 해둔다.
//builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();  // MVC 컨트롤러 사용
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // 개발자 모드에서 상세 오류 표시
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

#region 공지사항(NoticeApp) 관련 의존성(종속성) 주입 관련 코드만 따로 모아서 관리 
// ArticleAppDbContext.cs Inject: New DbContext Add
//builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
builder.Services.AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient); // 서비스 수명주기: Transient로 중복 호출시 사용..
//builder.Services.AddDbContextFactory<ArticleAppDbContext>(options => options.UseSqlServer(connectionString),
//    ServiceLifetime.Scoped  // 명시적 스코프 지정
//builder.Services.AddDbContextFactory<IdeaAppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContextFactory<IdeaAppDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddScoped<IdeaAppDbContext>(provider =>
//{
//    var factory = provider.GetRequiredService<IDbContextFactory<IdeaAppDbContext>>();
//    return factory.CreateDbContext();
//}
//);


// IArticleRepository.cs Inject: DI Container에 서비스(리포지토리) 등록
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

// DI Container에 서비스 등록
builder.Services.AddTransient<IVideoRepository, VideoRepositoryEfCoreAsync>();
//builder.Services.AddSingleton<IVideoRepositoryAsync>(new VideoRepositoryAdoNetAsync(connectionString));
//builder.Services.AddSingleton<IVideoRepositoryAsync>(new VideoRepositoryDapperAsync(connectionString));

builder.Services.AddTransient<IMediaRepository, MediaRepository>();
builder.Services.AddTransient<IMachineRepository, MachineRepository>();

//종속성주입 추가
builder.Services.AddTransient<IEmailSender, EmailSender>(); // EmailSender.cs
builder.Services.AddScoped<HttpClient>(); // MatBlazor 필수 문법

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

#region Serilog
//// 31.8.4. Serilog를 사용하여 로그 파일 기록하기 
//// ILoggerFactory loggerFactory
Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Debug()
    //.WriteTo.File(Path.Combine(app.Environment.ContentRootPath, "DnsLogs.txt"), rollingInterval: RollingInterval.Day)
    //.WriteTo.File("DnsLogs.txt", rollingInterval: RollingInterval.Day)
    //.ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Information("※※※[!] Blazor Server Application started.");
// Serilog를 DI 컨테이너에 추가
//builder.Host.UseSerilog();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
#endregion

var app = builder.Build();

// [2] Startup.Configure 영역 
// 개발 환경 설정
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    //개발환경에서 데이터가 없다면 데이터를 넣어라
    //04_06_CandidatesSeedData_Candidates 테이블에 기본 데이터 입력
    //https://www.youtube.com/watch?v=b7Ft2qRaEHU&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=17
    CandidateSeedData(app);

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//미들웨어 설정
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


//app.MapControllers();
app.MapDefaultControllerRoute();  //MVC 컨트롤러 라우팅
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


#region CandidateSeedData: Candidates 테이블에 기본 데이터 입력
// Candidates 테이블에 기본 데이터 입력
static void CandidateSeedData(WebApplication app)
{
    // https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/dependency-injection
    // ?view=aspnetcore-6.0#resolve-a-service-at-app-start-up
    using (var serviceScope = app.Services.CreateScope())
    {
        var services = serviceScope.ServiceProvider;

        var candidateDbContext = services.GetRequiredService<CandidateAppDbContext>();

        // Candidates 테이블에 데이터가 없을 때에만 데이터 입력
        if (!candidateDbContext.Candidates.Any())
        {
            candidateDbContext.Candidates.Add(
                new Candidate { FirstName = "길동", LastName = "홍", IsEnrollment = false });
            candidateDbContext.Candidates.Add(
                new Candidate { FirstName = "두산", LastName = "백", IsEnrollment = false });

            candidateDbContext.SaveChanges();
        }
    }
}
#endregion


