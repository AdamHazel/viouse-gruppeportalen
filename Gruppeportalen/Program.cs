using Gruppeportalen.Areas.CentralOrganisation.Services.Classes;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging; // For logging

var builder = WebApplication.CreateBuilder(args);

// Configure logging to show more detailed logs
builder.Logging.ClearProviders(); // Clear default providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPrivateUserOperations, PrivateUserOperations>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<ICentralOrganisationService, CentralOrganisationService>();

builder.Services.AddScoped<ILocalGroupService, LocalGroupService>();
builder.Services.AddScoped<ILocalGroupAdminService, LocalGroupAdminService>();
builder.Services.AddScoped<IOverviewService, OverviewService>();
builder.Services.AddScoped<IMembershipTypeService, MembershipTypeService>();
builder.Services.AddScoped<INorwayCountryInformation, NorwayCountryInformation>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IUserPersonConnectionsService, UserPersonConnectionsService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddTransient<IBraintreeService, BraintreeService>();

var app = builder.Build();

using (var services = app.Services.CreateScope()) 
{
    var db = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // For initialising roles and users at the start of program
    var um = services.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    ApplicationDbInitializer.Initialize(db, um);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
