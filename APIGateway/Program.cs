using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile("ocelot.json", true, true)
                        .AddJsonFile("/ocelot.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(s => {
                    s.AddOcelot().AddConsul();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging
                    .AddDebug()
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information);
                })
                .Configure(app =>
                {
                    app.UseOcelot().Wait();
                })
                .Build()
                .Run();
        }
    }
}
