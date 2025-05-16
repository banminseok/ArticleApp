using DotNetNote.Data;
using DotNetNote.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddTransient<IUrlRepository, UrlRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
//builder.Services.AddTransient<INoteCommentRepository, NoteCommentRepository>();
builder.Services.AddSingleton<INoteCommentRepository>(sp =>
    new NoteCommentRepository(
        connectionString, // 직접 전달
        sp.GetRequiredService<IMemoryCache>() // DI로 주입
    )
);

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
