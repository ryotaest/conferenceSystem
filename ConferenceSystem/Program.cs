using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConferenceSystem.data;
using Microsoft.AspNetCore.Authentication.Cookies;
using ConferenceSystem;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ConfeSystemData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConfeSystemData") ?? throw new InvalidOperationException("Connection string 'ConfeSystemData' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Cookies";
        options.LoginPath = "/Home/Login"; // The login page URL
        options.AccessDeniedPath = "/Home/AccessDenied"; // The access denied page URL
    });

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

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseCookiePolicy();

app.Run();
