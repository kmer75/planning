using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlanningApi.Auth;
using PlanningApi.Services;
using Repositories.Repositories;
using Repositories.Model;
using static PlanningApi.Helpers.Constants;
using AutoMapper;
using PlanningApi.DTO;

namespace PlanningApi.Controllers
{
    /// <summary>
    /// controller for testing the JWT with claims acccess (configured into startup file Authorization access section)
    /// </summary>
    [Authorize(Policy = "API_ACCESS")]
    [Authorize(Policy = "USER")]
    [Route("api/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller with the Depedency Injections
        /// </summary>
        /// <param name="UserService"></param>
        /// <param name="httpContextAccessor">the current context (can get the current Principal)</param>
        public DashboardController(IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// A test API to work with JWT
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Home()
        {

            var userId = _caller.Claims.Single(c => c.Type == JwtClaimIdentifiers.Id);
            var id = userId.Value;
            var user = _userService.FindByIdAsync(id);


            return new OkObjectResult(new
            {
                User = user
            });
        }
    }
}
