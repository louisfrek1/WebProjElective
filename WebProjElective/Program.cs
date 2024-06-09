using Microsoft.AspNetCore.Authentication.Cookies;
using WebProjElective.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<DbContext>(provider =>
//{
//    var configuration = provider.GetRequiredService<IConfiguration>();
//    var connectionString = configuration.GetConnectionString("DefaultConnection");
//    return new DbContext(connectionString);
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<UserContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new UserContext(connectionString);
});

builder.Services.AddTransient<ProductContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new ProductContext(connectionString);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index"; // Redirect to login page if not authenticated
        options.LogoutPath = "/Home/Logout";
    });

var app = builder.Build();

// Ensure database connection works at startup
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var context = services.GetRequiredService<DbContext>();
//        bool isConnected = context.TestConnection();
//        if (isConnected)
//        {
//            Console.WriteLine("Successfully connected to the database.");
//        }
//        else
//        {
//            Console.WriteLine("Failed to connect to the database.");
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"An error occurred while checking the database: {ex.Message}");
//    }
//}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
