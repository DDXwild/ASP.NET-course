using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Identity.Entities;
using SurveyPortal.Models.Identity;
using SurveyPortal.Models.Survey.Survey;
using SurveyPortal.Models;
using SurveyPortal.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<SurveyDbContext>(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("SurveyPortalConnection");
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddDbContext<AppIdentityDbContext>(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
    options.AccessDeniedPath = "/Accounts/AccessDenied";
});

builder.Services.AddScoped<ISurveyRepository, EFSurveyRepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Home/Index/{page:int}",
    defaults: new { Controller = "Home", action = "Index" }
);

SeedData.EnsurePopulated(app);

var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
string[] roles = { "Visitor", "Admin" };

foreach (var role in roles)
{
    if (!await roleManager.RoleExistsAsync(role))
    {
        await roleManager.CreateAsync(new IdentityRole(role));
    }
}

var adminEmail = builder.Configuration["AdminUser:Email"] ?? "admin@surveyportal.local";
var adminPassword = builder.Configuration["AdminUser:Password"] ?? "Admin1234";
var adminFirstName = builder.Configuration["AdminUser:FirstName"] ?? "System";
var adminLastName = builder.Configuration["AdminUser:LastName"] ?? "Admin";

var adminUser = await userManager.FindByEmailAsync(adminEmail);
if (adminUser is null)
{
    adminUser = new User
    {
        UserName = adminEmail,
        Email = adminEmail,
        FirstName = adminFirstName,
        LastName = adminLastName,
        Address = string.Empty,
        City = string.Empty,
        State = string.Empty,
        PostalCode = string.Empty,
        Country = string.Empty,
        ProfilePictureUrl = string.Empty
    };

    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
    if (createResult.Succeeded)
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
{
    await userManager.AddToRoleAsync(adminUser, "Admin");
}

app.Run();
