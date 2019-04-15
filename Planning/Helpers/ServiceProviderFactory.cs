using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Helpers
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Instance { get; set; }
    }
}
