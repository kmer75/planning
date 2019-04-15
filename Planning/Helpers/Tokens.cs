using Newtonsoft.Json;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlanningApi.Auth;
using PlanningApi.Auth.Models;
using static PlanningApi.Helpers.Constants;

namespace PlanningApi.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(
            ClaimsIdentity identity, 
            IJwtFactory jwtFactory, 
            JwtIssuerOptions jwtOptions, 
            JsonSerializerSettings serializerSettings)
        {
            var response = new JsonWebToken()
            {
                AccessToken = await jwtFactory.GenerateEncodedToken(identity),
                Expires = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }

        
    }
}
