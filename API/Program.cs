using API.Services;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7000";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

//https://docs.identityserver.io/en/latest/quickstarts/1_client_credentials.html

//.AddIdentityServerAuthentication("Bearer", options =>
//{
//    options.Authority = "https://localhost:7000"; //Identity server projects
//    options.ApiName = "api1";


//});

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
