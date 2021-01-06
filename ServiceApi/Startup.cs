using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Consul;
using ConsulInfrastructure;

namespace ServiceApi
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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConsulService consulService = new ConsulService();
            Uri url;
            if(Uri.TryCreate(Configuration["Consul:ServiceUrl"], UriKind.Absolute,out url))
            {
                consulService.ServiceUrl = url;
            }
            
            HealthService healthService = new HealthService()
            {
                InstanceName = Configuration["InstanceName"] ?? Configuration["Service:IP"] + ':' + Configuration["Service:Port"],
                IP = Configuration["Service:IP"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
            };
            app.RegisterConsul(lifetime, healthService, consulService);
        }
    }
}
