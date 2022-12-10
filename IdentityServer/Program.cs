using IdentityServer;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
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



// Add services to the container.
builder.Services.AddRazorPages();

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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

//app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
