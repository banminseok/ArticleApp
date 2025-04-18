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
//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();  // MVC ��Ʈ�ѷ� ���
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // ������ ��忡�� �� ���� ǥ��
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

#region ��������(NoticeApp) ���� ������(���Ӽ�) ���� ���� �ڵ常 ���� ��Ƽ� ���� 
// ArticleAppDbContext.cs Inject: New DbContext Add
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient); // ���� �����ֱ�: Transient�� �ߺ� ȣ��� ���..

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

//���Ӽ����� �߰�
builder.Services.AddTransient<IEmailSender,EmailSender>(); // EmailSender.cs

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapDefaultControllerRoute();  //MVC ��Ʈ�ѷ� �����
app.MapControllerRoute(
    name : "default",
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


