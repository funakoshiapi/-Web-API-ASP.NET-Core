using TaxEngineAPI.Extensions;
using TaxEngineAPI.Entity;
using TaxEngineAPI.Entity.Models;
using TaxEngineMongoDbRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TaxMappingRepository;
using TaxMappingRepository.Models;

namespace TaxEngineAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.AddDbContext<TaxEngineDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TaxEngineDb")));
            services.AddDbContext<MappingDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AvTaxEngineLocalDb")));
            services.AddScoped<ITaxEngineRepository, TaxEngineRepository>();
            services.AddScoped<IMappingRepository, MappingRepository>();
            services.AddScoped<ITtrRepository, TtrRepository>();
                        
            services.AddControllers(setupAction =>
                setupAction.ReturnHttpNotAcceptable = true
            )
            .AddNewtonsoftJson();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "TaxEngineAPI", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxEngineAPI v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            //enables using static files for the request
            app.UseStaticFiles();
            
            //will forward proxy headers to the current request. useful during application deployment.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
