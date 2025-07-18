using ArticleApp.Models;
using ArticleApp.Models.Categories;
using ArticleApp.Models.Products;
using Blazored.Toast;
using Hawaso.Areas.Identity;
using Hawaso.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<DotNetNoteContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<CommonValueDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ArticleAppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Transient); // ���� �����ֱ�: Transient�� �ߺ� ȣ��� ���..
// Add HawasoDbContext ����
builder.Services
    .AddEntityFrameworkSqlServer()
    .AddDbContext<HawasoDbContext>(options => 
        options.UseSqlServer(connectionString),ServiceLifetime.Transient);


builder.Services.AddControllersWithViews();  //MVC+ Web API ��밡�� //MVC
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

//install-Package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddTransient<INoteRepository, NoteRepository>();
builder.Services.AddTransient<INoteCommentRepository, NoteCommentRepository>();
builder.Services.AddTransient<ICommonValueRepository, CommonValueRepository>(); //CommonValue
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>(); //Customer
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>(); //Category
builder.Services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>(); //
builder.Services.AddTransient<IMachineTypeRepository, MachineTypeRepository>(); //MachineType

builder.Services.AddDependencyInjectionContainerForManufacturer(connectionString);
// Blazored.Toast
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
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


#region CORS
// CORS ��å ���� Anglular, React, Vue.js �� Ŭ���̾�Ʈ �ۿ��� ȣ�� ����
app.UseCors(); // CORS ��å ����// �ݵ�� UseRouting() ������ ȣ���ؾ� �� 
#endregion



app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
