using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanningApi.Auth;
using PlanningApi.Helpers;
using Repositories.Repositories;
using Repositories.Model;

namespace PlanningApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region proprietes

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtFactory _jwtFactory;

        #endregion

        #region constructor

        public AuthenticationService(
            UserManager<User> userManager, 
            RoleManager<ApplicationRole> roleManager, IJwtFactory jwtFactory, 
            SignInManager<User> signInManager
            )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        #endregion


        #region methodes

        /// <summary>
        /// get current user claims
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName)
        {
            var userToVerify = this._userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            
            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify.Result));
        }
        
        /// <summary>
        /// it will verify if the user mail is confirmed, or is locked
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <param name="lockoutOnFailure"></param>
        /// <returns></returns>
        public async Task<SignInResult> SignInAsync(string username, string password, bool rememberMe = true, bool lockoutOnFailure = false)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
            return result;
        }

        /// <summary>
        /// Not used because we use JWT
        /// </summary>
        public void SignOutAsync()
        {
            _signInManager.SignOutAsync();
        }
        

        public async Task<User> FindByEmailAsync(string email)
        {
            return await this._userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindByIdAsync(string Id)
        {
            return await this._userManager.FindByIdAsync(Id);
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return await this._userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await this._userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            return await this._userManager.ResetPasswordAsync(user, token, newPassword);
        }

        #endregion

    }
}
