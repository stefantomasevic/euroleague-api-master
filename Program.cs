using Euroleague;
using Euroleague.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); 
});


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseSwagger();

startup.Configure(app, builder.Environment);


app.MapControllers();



app.Run();






