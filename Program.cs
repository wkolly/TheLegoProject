using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLegoProject.Data;
using TheLegoProject.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<LegoDatabase2Context>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings=legoConnection"]);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 14;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
    .AddRoles<IdentityRole>() // Make sure to add roles support
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});

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
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();

app.Use (async (ctx, next) =>
{
    string csp = "default-src 'self'; " +
                 "script-src 'self' https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js 'unsafe-inline' https://apis.google.com; " +
                 "script-src-elem 'self' https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js 'unsafe-inline' https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js; " +
                 "connect-src 'self' 'unsafe-inline' ws://localhost:57798 http://localhost:57798 https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js ws://localhost:62719 http://localhost:62719 wss://localhost:44300; " +
                 "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap; " +
                 "font-src 'self' https://fonts.gstatic.com; " +
                 "img-src 'self' data: https://m.media-amazon.com https://www.lego.com https://images.brickset.com https://www.brickeconomy.com;";
    if (ctx.Request.IsHttps || app.Environment.IsDevelopment())
    {
        ctx.Response.Headers.Add("Content-Security-Policy", csp);
    }

    await next.Invoke();
});

app.UseAuthentication(); // Make sure to call UseAuthentication before UseAuthorization.
app.UseAuthorization();

app.MapControllerRoute("pagination", "Products/{pageNum}", new { Controller = "Home", action = "Products"});
app.MapDefaultControllerRoute();

//app.MapControllerRoute(
    //name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();