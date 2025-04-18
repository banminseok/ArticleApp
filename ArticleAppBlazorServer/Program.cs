using ArticleApp.Models;
using ArticleAppBlazorServer.Areas.Identity;
using ArticleAppBlazorServer.Areas.Identity.Models;
using ArticleAppBlazorServer.Areas.Identity.Services;
using ArticleAppBlazorServer.Data;
using ArticleAppBlazorServer.Managers;
using ArticleAppBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UploadApp.Shared;

var builder = WebApplication.CreateBuilder(args);

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
//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();  // MVC 컨트롤러 사용
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // 개발자 모드에서 상세 오류 표시
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

#region 공지사항(NoticeApp) 관련 의존성(종속성) 주입 관련 코드만 따로 모아서 관리 
// ArticleAppDbContext.cs Inject: New DbContext Add
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient); // 서비스 수명주기: Transient로 중복 호출시 사용..

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

//종속성주입 추가
builder.Services.AddTransient<IEmailSender,EmailSender>(); // EmailSender.cs

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapDefaultControllerRoute();  //MVC 컨트롤러 라우팅
app.MapControllerRoute(
    name : "default",
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


