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

// 사용자 정의한 DotNetNoteSettings.json 추가  DotNetNoteSettings.cs 과 쌍으로 이루어짐
builder.Configuration.AddJsonFile($"Settings\\DotNetNoteSettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // 환경 변수 추가
// 이후 builder.Configuration["키"] 또는 GetSection("섹션")으로 접근 가능

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

#region Web 프로젝트별 Services
builder.Services.AddControllersWithViews();  //MVC+ Web API 사용가능
builder.Services.AddRazorPages(); // Razor Pages 사용가능
builder.Services.AddServerSideBlazor() // Blazor Server 사용가능
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); // 개발자 모드에서 상세 오류 표시

#endregion

builder.Services.Configure<DotNetNoteSettings>(builder.Configuration.GetSection("DotNetNoteSettings"));
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login/";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // 쿠키 만료 시간 설정        
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
//[User][9] Policy 설정
builder.Services.AddAuthorization(options =>
{
    //Users Role 이 있으면 Users Policy 부여
    options.AddPolicy("Users", policy =>
    {
        policy.RequireRole("Users");
    });
    //Users Role 이 있고 UserId가 "Admin"인 경우에만 Administrators Policy 부여
    options.AddPolicy("Administrators", policy =>
    {
        policy.RequireRole("Users")
                .RequireClaim("UserId",//대소문자구분
                    builder.Configuration.GetSection("DotNetNoteSettings")
                    .GetSection("SiteAdmin").Value);
    });
});

builder.Services.AddTransient<IUrlRepository, UrlRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
//builder.Services.AddTransient<INoteCommentRepository, NoteCommentRepository>();
builder.Services.AddSingleton<INoteCommentRepository>(sp =>
    new NoteCommentRepository(
        connectionString, // 직접 전달
        sp.GetRequiredService<IMemoryCache>() // DI로 주입
    )
);
builder.Services.AddTransient<IRecruitSettingRepository, RecruitSettingRepository>();
builder.Services.AddTransient<IRecruitRegistrationRepository, RecruitRegistrationRepository>();
// 포인트 관리
//builder.Services.AddTransient<IPointRepository, PointRepository>(); // DB 사용
builder.Services.AddTransient<IPointRepository, PointRepositoryInMemory>(); // 인-메모리 사용
builder.Services.AddTransient<IPointLogRepository, PointLogRepository>();
//Five 컴포넌트
builder.Services.AddTransient<IFiveRepository, FiveRepository>();

//DI(의존성 주입) 컨테이너에 서비스 등록
builder.Services.AddTransient<CopyrightService>();
builder.Services.AddTransient<ICopyrightService, CopyrightService>();

builder.Services.AddSwaggerGen(c => 
{ 
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

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
Log.Information("※※※[!] DotnetNote Application started.");
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// CORS 사용 허용
app.UseCors("AllowAnyOrigin");
app.UseCors(options => options.WithOrigins("https://example.com"));//특정 출처 허용 (예: example.com)
    //.AllowAnyOrigin() // 모든 출처 허용
    //.AllowAnyMethod() // 모든 HTTP 메서드 허용 (GET, POST, PUT, DELETE 등)
    //.AllowAnyHeader()); // 모든 헤더 허용

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
app.UseSession(); // 세션 미들웨어 사용
                  



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
   .WithStaticAssets();  //정적 자산 사용 (특정 엔드포인트에서만 정적 파일을 제공할 수 있습니다.)
//Blazor Server 사용가능
app.MapBlazorHub();

app.Run();
