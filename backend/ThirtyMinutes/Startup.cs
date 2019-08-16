using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ThirtyMinutes.Persistence;
using ThirtyMinutes.Persistence.InMemory;
using ThirtyMinutes.Persistence.Mongo;

namespace ThirtyMinutes
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        bool useInMemoryDatabase = true;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(hostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true) //load local
               .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);

            if (hostingEnvironment.IsDevelopment())
            {
                //builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            });


            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/default-settings?view=aspnetcore-2.1
            services.AddDataProtection();

            services.Configure<CookiePolicyOptions>(options => { options.CheckConsentNeeded = context => false; });

            // mongo
            if (!useInMemoryDatabase)
            {
                services.AddSingleton(new MongoDatabaseOptions(Configuration.GetConnectionString("Mongo"),
                    Configuration.GetValue<string>("appSettings:MongoDatabaseName")));
                services.AddScoped<IGameRepository, MongoGameRepository>();
                services.AddScoped<IGameSessionRepository, MongoGameSessionRepository>();
            }

            if (useInMemoryDatabase)
            {
                services.AddScoped<IGameRepository, TestGameRepository>();
                services.AddSingleton<IGameSessionRepository, TestGameSessionRepository>();
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Thirty minutes backend", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thirty minutes backend API V1"); });
        }
    }
}