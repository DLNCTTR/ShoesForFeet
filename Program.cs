using Microsoft.EntityFrameworkCore;
using ShoesForFeet.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Redirect to Login page if unauthorized
        options.LogoutPath = "/User/Logout"; // Redirect to Logout page on logout
        options.Cookie.Name = "ShoesForFeetAuth"; // Custom cookie name
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set cookie expiration time
        options.SlidingExpiration = true; // Enable sliding expiration
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use custom error handling in production
}
else
{
    app.UseDeveloperExceptionPage(); // Enable detailed error page in development
}

app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting(); // Enable routing
app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization

// Map routes to controllers
app.MapDefaultControllerRoute();

app.Run();