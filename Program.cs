using Euroleague;
using Euroleague.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://euroleague.onrender.com") // Vaša frontend adresa
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();

startup.Configure(app, builder.Environment);


app.MapControllers();



app.Run();






