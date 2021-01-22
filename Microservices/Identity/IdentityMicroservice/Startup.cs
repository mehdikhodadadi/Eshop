using HealthChecks.UI.Client;
using Repository = IdentityRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Middleware;
using MongoDB.Driver;

namespace IdentityMicroservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMongoDb(Configuration);
            services.AddSingleton<Repository.IUserRepository>(sp => new Repository.UserRepository(sp.GetService<IMongoDatabase>()));
            services.AddJwt(Configuration);
            services.AddTransient<IEncryptor, Encryptor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddMongoDb(
                                 mongodbConnectionString: Configuration.GetSection("mongo").Get<MongoOptions>().ConnectionString,
                                 name: "mongo",
                                 failureStatus: HealthStatus.Unhealthy
                               );

            services.AddHealthChecksUI().AddInMemoryStorage();
        }
    }
}