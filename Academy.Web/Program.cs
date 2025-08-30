using Academy.Application.Services;
using Academy.Core.Interfaces;
using Academy.Infrastructure.Data;
using Academy.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure configuration sources
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(options =>
{
    // Ensure controllers use the same view engine as Razor Pages
});

// Configure Razor view engine for both Pages and Controllers
builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
});

// Add Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<AcademyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

// Add Services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add Response Caching
builder.Services.AddResponseCaching();

// Add Session Support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Add Email Service
builder.Services.AddScoped<Academy.Web.Services.EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseResponseCaching();

// Use Session
app.UseSession();

// Use CORS
app.UseCors("AllowSpecificOrigins");

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();

app.MapControllers();



app.Run();
