using Euroleague.Data;
using Euroleague.Mappings;
using Euroleague.Repository;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Euroleague
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://euroleague.onrender.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            // konekcioni
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = "Settings.config"
            };
            Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            string localConnection = config.AppSettings.Settings["Database:Local"]?.Value;
            string liveConnection = config.AppSettings.Settings["Database:Live"]?.Value;

            string connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? localConnection
                : liveConnection;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information); 
            });

            services.AddScoped<ITeamRepository, SqlTeamRepository>();
            services.AddScoped<IPlayerRepository, SqlPlayerRepository>();
            services.AddScoped<IScheduleRepository, SqlScheduleRepository>();
            services.AddScoped<IStatisticRepository, SqlStatisticRepository>();

            services.AddAutoMapper(typeof(AutoMapperprofile));

           




            services.AddControllers();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

          

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourApiName v1"));
            }



            app.UseCors("CorsPolicy");
            app.UseStaticFiles();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Startup>>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }
            app.UseRouting();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



           
        }
    }
}
