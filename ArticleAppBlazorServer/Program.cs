using ArticleApp.Models;
using ArticleAppBlazorServer.Areas.Identity;
using ArticleAppBlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

// ArticleAppDbContext.cs Inject: New DbContext Add
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// IArticleRepository.cs Inject: DI Container�� ����(�������丮) ���
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();

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
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
