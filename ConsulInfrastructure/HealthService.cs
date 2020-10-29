using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulInfrastructure
{
    public class HealthService
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
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
