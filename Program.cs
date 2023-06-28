using AniversaryApp.Data;
using AniversaryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid;


var builder = WebApplication.CreateBuilder(args);
var servicies = builder.Services;
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
servicies.AddSingleton(configuration);

servicies.AddTransient<ISendGridClient>(_ => new SendGridClient(configuration["SendGrid:ApiKey"]));

servicies.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

//servicies.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Middleware for login redirect
app.Use(async (context, next) =>
{
    var user = context.User;
    if (user.Identity != null && !user.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/Home"))
    {
        context.Response.Redirect("/Home/LandPage");
        return;
    }

    await next();
});
app.Run();
//dotnet ef database update --project B:\proiecteVS\AniversaryAppSolution\AniversaryApp\AniversaryApp.csproj
//dotnet ef migrations add InitialMigration --project B:\proiecteVS\AniversaryAppSolution\AniversaryApp\AniversaryApp.csproj