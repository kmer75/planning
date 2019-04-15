using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanningApi.Auth;
using PlanningApi.Helpers;
using Repositories.Repositories;
using Repositories.Model;
using AutoMapper;

namespace PlanningApi.Services
{
    public class RegisterService : IRegisterService
    {
        #region proprietes

        private readonly UserManager<User> _userManager;
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        #endregion

        #region constructor

        public RegisterService(UserManager<User> userManager, IUserRepository userRepository,IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        #endregion


        #region methodes

        /// <summary>
        /// Create an Identity user, it will encrypt the password
        /// </summary>
        /// <param name="user">Application user (Identity User)</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateUserIdentityAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// assign list of roles to an user. the user is found by his username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<IdentityResult> SetRoleToUserAsync(string username, List<string> roles)
        {
            var user = this._userManager.FindByNameAsync(username);
            return this._userManager.AddToRolesAsync(user.Result, roles);
        }
        /// <summary>
        /// generate an email confirmation that is needed to be connected (if SignIn.RequireConfirmedEmail option is true)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return this._userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// find an Identity user by his Id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<User> GetUserByIdAsync(string userid)
        {
            return _userManager.FindByIdAsync(userid);
        }

        /// <summary>
        /// find an Identity user by his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// This will update EmailConfirmed field in Database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            IdentityResult result = await this._userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        #endregion

    }
}
