using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SingleTicketing.Data;
using SingleTicketing.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Logging.AddConsole();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 21))));
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>(); // R
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Configure Kestrel to listen on all network interfaces before building the app
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // For HTTP
    serverOptions.ListenAnyIP(5001); // For HTTPS
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DriverSeeder.SeedDrivers(services);
    }
    catch (Exception ex)
    {
        // Handle exceptions (e.g., log error)
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseMiddleware<SingleTicketing.Middleware.PageVisitLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
