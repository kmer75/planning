﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Repositories.Model;

namespace PlanningApi.Auth
{
    public interface IJwtFactory
    {
        /// <summary>
        /// generate an encoded token with some claims that we are able to decode ( https://jwt.io/ ) 
        /// </summary>
        /// <param name="identity">identity Claims generated by _jwtFactory.GenerateClaimsIdentity(string iduser, User U)</param>
        /// <returns></returns>
        Task<string> GenerateEncodedToken(ClaimsIdentity identity);

        /// <summary>
        /// it generates the user claims. add what you need as claims here
        /// </summary>
        /// <param name="Id">ID User</param>
        /// <param name="user">Identity user</param>
        /// <returns></returns>
        ClaimsIdentity GenerateClaimsIdentity(User user);

    }
}