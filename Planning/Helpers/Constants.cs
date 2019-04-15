using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Helpers
{
    public class Constants
    {
        public static class JwtClaimIdentifiers
        {
            public const string Id = "id";
            public const string IdIdentity = "id_identity";
        }

        public static class JwtClaims
        {
            public const string ApiAccess = "api_access";
        }

        public static class Sex
        {
            public const int Male = 1;
            public const int Female = 2;
        }
    }
}
