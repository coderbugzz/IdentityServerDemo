using IdentityServer;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IndentityServer;
using IndentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
         b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();


builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();



var app = builder.Build();

#region Initialized Database
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    context.Database.Migrate();
    if (!context.Clients.Any())
    {
        foreach (var client in Config.GetClients())
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.IdentityResources.Any())
    {
        foreach (var resource in Config.GetIdentityResources())
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.ApiScopes.Any())
    {
        foreach (var resource in Config.ApiScopes.ToList())
        {
            context.ApiScopes.Add(resource.ToEntity());
        }

        context.SaveChanges();
    }

    if (!context.ApiResources.Any())
    {
        foreach (var resource in Config.GetApis())
        {
            context.ApiResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
}
#endregion

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.Run();
