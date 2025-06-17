using DotNetNote.Controllers;
using DotNetNote.Data;
using DotNetNote.Models;
using DotNetNote.Models.RecruitManager;
using DotNetNote.Services;
using DotNetNote.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ����� ������ DotNetNoteSettings.json �߰�  DotNetNoteSettings.cs �� ������ �̷����
builder.Configuration.AddJsonFile($"Settings\\DotNetNoteSettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // ȯ�� ���� �߰�
// ���� builder.Configuration["Ű"] �Ǵ� GetSection("����")���� ���� ����

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<DotNetNoteContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDbContextFactory<DotNetNoteContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

#region Web ������Ʈ�� Services
builder.Services.AddControllersWithViews();  //MVC+ Web API ��밡��
builder.Services.AddRazorPages(); // Razor Pages ��밡��
builder.Services.AddServerSideBlazor() // Blazor Server ��밡��
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // ������ ��忡�� �� ���� ǥ��

#endregion

builder.Services.Configure<DotNetNoteSettings>(builder.Configuration.GetSection("DotNetNoteSettings"));
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login/";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // ��Ű ���� �ð� ����        
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
     {
         options.RequireHttpsMetadata = false;
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateAudience = false,
             ValidateIssuer = false,
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])),
             ValidateLifetime = true,
             ClockSkew = TimeSpan.FromMinutes(5)
         };
     });
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//[User][9] Policy ����
builder.Services.AddAuthorization(options =>
{
    //Users Role �� ������ Users Policy �ο�
    options.AddPolicy("Users", policy =>
    {
        policy.RequireRole("Users");
    });
    //Users Role �� �ְ� UserId�� "Admin"�� ��쿡�� Administrators Policy �ο�
    options.AddPolicy("Administrators", policy =>
    {
        policy.RequireRole("Users")
                .RequireClaim("UserId",//��ҹ��ڱ���
                    builder.Configuration.GetSection("DotNetNoteSettings")
                    .GetSection("SiteAdmin").Value);
    });
});

builder.Services.AddTransient<IUrlRepository, UrlRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
//builder.Services.AddTransient<INoteCommentRepository, NoteCommentRepository>();
builder.Services.AddSingleton<INoteCommentRepository>(sp =>
    new NoteCommentRepository(
        connectionString, // ���� ����
        sp.GetRequiredService<IMemoryCache>() // DI�� ����
    )
);
builder.Services.AddTransient<IRecruitSettingRepository, RecruitSettingRepository>();
builder.Services.AddTransient<IRecruitRegistrationRepository, RecruitRegistrationRepository>();
// ����Ʈ ����
//builder.Services.AddTransient<IPointRepository, PointRepository>(); // DB ���
builder.Services.AddTransient<IPointRepository, PointRepositoryInMemory>(); // ��-�޸� ���
builder.Services.AddTransient<IPointLogRepository, PointLogRepository>();
//Five ������Ʈ
builder.Services.AddTransient<IFiveRepository, FiveRepository>();

//DI(������ ����) �����̳ʿ� ���� ���
builder.Services.AddTransient<CopyrightService>();
builder.Services.AddTransient<ICopyrightService, CopyrightService>();

builder.Services.AddSwaggerGen(c => 
{ 
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

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
Log.Information("�ءء�[!] DotnetNote Application started.");
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// CORS ��� ���
app.UseCors("AllowAnyOrigin");
app.UseCors(options => options.WithOrigins("https://example.com"));//Ư�� ��ó ��� (��: example.com)
    //.AllowAnyOrigin() // ��� ��ó ���
    //.AllowAnyMethod() // ��� HTTP �޼��� ��� (GET, POST, PUT, DELETE ��)
    //.AllowAnyHeader()); // ��� ��� ���

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // ���� �̵���� ���
                  



app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();  //���� �ڻ� ��� (Ư�� ��������Ʈ������ ���� ������ ������ �� �ֽ��ϴ�.)
//Blazor Server ��밡��
app.MapBlazorHub();

app.Run();
