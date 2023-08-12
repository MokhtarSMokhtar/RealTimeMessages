using API.Data;
using API.Entities;
using DatingApi.Data;
using DatingApi.Entities;
using DatingApi.Extentions;
using DatingApi.Interface;
using DatingApi.MiddleWare;
using DatingApi.NoSql;
using DatingApi.Services;
using DatingApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdintityService(builder.Configuration);
builder.Services.AddNoSqlService(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowConnection", builder =>
    {
        builder.WithOrigins("https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleWare>();
app.UseCors("allowConnection");
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
  app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/Message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUser(userManager, roleManager);
}
catch(Exception ex)
{ 
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An Error occurred during migration");
}

app.Run();
