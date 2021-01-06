using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulInfrastructure
{
    public class HealthService
    {
        public string InstanceName { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// port
        /// </summary>
        public int Port { get; set; }

    }
}
