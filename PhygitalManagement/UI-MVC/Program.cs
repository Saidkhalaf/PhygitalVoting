using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using PM.DAL.EF;
using PM.UI.Web.MVC.Resources;
using Microsoft.AspNetCore.Identity;
using PM.BL.flowManager;
using PM.BL.projectManager;
using PM.BL.questionManager;
using PM.BL.subthemeManager;
using PM.BL.SubthemeManager;
using PM.DAL.EF.flowRepository;
using PM.DAL.EF.projectRepository;
using PM.DAL.EF.questionRepository;
using PM.DAL.EF.subPlatformAdminRepository;
using PM.DAL.EF.subthemeRepository;
using PM.UI.Web.MVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddDbContext<PhygitalDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PhygitalDbContext>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionManager, QuestionManager>();

builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IFlowManager, FlowManager>();

builder.Services.AddScoped<ISubthemeRepository, SubthemeRepository>();
builder.Services.AddScoped<ISubthemeManager, SubthemeManager>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectManager, ProjectManager>();

builder.Services.AddScoped<ISubPlatformAdminRepository, SubPlatformAdminRepository>();

builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddMvc()
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResouce));
    });
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
 
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "nl", "fr"};
    options.SetDefaultCulture(supportedCultures[1])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

// Configure logging
builder.Logging.AddConsole();

var app = builder.Build();

app.UseRequestLocalization();

void ConfigureServices(IServiceCollection services)
{

    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy =>
        {
            policy.RequireRole("Admin");
        });

    });
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PhygitalDbContext>();
    if (context.CreateDatabase(dropDatabase: true))
    {
        // Add users & roles
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();
        SeedIdentity(userManager, roleManager);

        DataSeeder.Seed(context);
    }
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "language",
    pattern: "{controller=Home}/{action=SetLanguage}/{culture?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Question}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void SeedIdentity(UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    //seed roles
    var adminRole = new IdentityRole(CustomIdentityConstants.AdminRole);
    roleManager.CreateAsync(adminRole).Wait();

    var subPlatformAdminRole = new IdentityRole(CustomIdentityConstants.SubPlatformAdminRole);
    roleManager.CreateAsync(subPlatformAdminRole).Wait();
    
    var userRole = new IdentityRole(CustomIdentityConstants.UserRole);
    roleManager.CreateAsync(userRole).Wait();
    
    //seed users
    var stemconnect = new IdentityUser
    {
        UserName = "stemconnect@kdg.be",
        Email = "stemconnect@kdg.be",
        EmailConfirmed = true
    };
    userManager.CreateAsync(stemconnect, "Stemconnect1!").Wait();
    userManager.AddToRoleAsync(stemconnect, CustomIdentityConstants.AdminRole).Wait();
    
    var subadmin1 = new IdentityUser
    {
        UserName = "subadmin1@kdg.be",
        Email = "subadmin1@kdg.be",
        EmailConfirmed = true
    };
    userManager.CreateAsync(subadmin1, "Subadmin1!").Wait();
    userManager.AddToRoleAsync(subadmin1, CustomIdentityConstants.SubPlatformAdminRole).Wait();
    
    var user = new IdentityUser
    {
        UserName = "user@kdg.be",
        Email = "user@kdg.be",
        EmailConfirmed = true
    };
    userManager.CreateAsync(user, "Password1!").Wait();
    userManager.AddToRoleAsync(user, CustomIdentityConstants.UserRole).Wait();
}
public partial class Program { }
