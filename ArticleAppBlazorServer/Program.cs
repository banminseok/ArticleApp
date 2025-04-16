using ArticleApp.Models;
using ArticleAppBlazorServer.Areas.Identity;
using ArticleAppBlazorServer.Data;
using ArticleAppBlazorServer.Managers;
using ArticleAppBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UploadApp.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();  // MVC 컨트롤러 사용
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // 개발자 모드에서 상세 오류 표시
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
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
app.MapDefaultControllerRoute();  //MVC 컨트롤러 라우팅
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
