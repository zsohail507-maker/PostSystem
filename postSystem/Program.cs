using Microsoft.EntityFrameworkCore;
using postSystem.Models.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Services
// =======================

// MVC
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<MasterDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionStringF")));

// Session (Admin login)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Security headers (recommended for AdSense + production)
builder.Services.AddAntiforgery();

var app = builder.Build();

// =======================
// Middleware Pipeline
// =======================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session must be BEFORE Authorization
app.UseSession();

app.UseAuthorization();



using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MasterDBContext>();

// Apply migrations (optional but recommended)
db.Database.Migrate();

// Seed admin
AdminSeeder.Seed(db);

// =======================
// Routing
// =======================

// SEO-friendly post route
app.MapControllerRoute(
    name: "post",
    pattern: "post/{slug}",
    defaults: new { controller = "Post", action = "Details" });

// Admin area
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

// Default route (Public blog)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
