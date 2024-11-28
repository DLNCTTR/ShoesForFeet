var builder = WebApplication.CreateBuilder(args);

// Register services needed for MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Error/Error404");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "product",
        pattern: "Product/{action=List}/{id?}",
        defaults: new { controller = "Product" });

    endpoints.MapControllerRoute(
        name: "user",
        pattern: "User/{action=List}/{id?}",
        defaults: new { controller = "User" });
});

app.Run();