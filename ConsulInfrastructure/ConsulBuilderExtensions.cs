using Consul;
using Microsoft.AspNetCore.Builder;
using System;

namespace ConsulInfrastructure
{
    public static class ConsulBuilderExtensions
    {

        // Service registration

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime, HealthService healthService, ConsulService consulService)
        {

            var consulClient = new ConsulClient(x => x.Address = consulService.ServiceUrl);//Request to register for Consul address

            var httpCheck = new AgentServiceCheck()

            {

                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//How long does the service start to register?

                Interval = TimeSpan.FromSeconds(10), // health check interval, or heartbeat interval

                HTTP = $"https://{healthService.IP}:{healthService.Port}/api/health", // Health Check Address

                Timeout = TimeSpan.FromSeconds(5)

            };

            // Register service with consul

            var registration = new AgentServiceRegistration()

            {

                Checks = new[] { httpCheck },

                ID = healthService.Name + "_" + healthService.Port,

                Name = healthService.Name,

                Address = healthService.IP,

                Port = healthService.Port,

                Tags = new[] { $"urlprefix-/{healthService.Name}" }//Add a tag tag in urlprefix-/servicename format for Fabio to recognize

            };

            consulClient.Agent.ServiceRegister(registration).Wait();//The service is registered at startup. The internal implementation is actually registered using the Consul API (initiated by HttpClient).

            lifetime.ApplicationStopping.Register(() =>

            {

                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//Unregister when the service is stopped

            });

            return app;

        }
    }
}