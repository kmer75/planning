using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Repositories;
using Repositories.Model;

namespace PlanningApi.Services
{
    public interface IRegisterService
    {
        /// <summary>
        /// Create an Identity user, it will encrypt the password
        /// </summary>
        /// <param name="user">Application user (Identity User)</param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateUserIdentityAsync(User user, string password);
        /// <summary>
        /// assign list of roles to an user. the user is found by his username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<IdentityResult> SetRoleToUserAsync(string username, List<string> roles);
        /// <summary>
        /// generate an email confirmation that is needed to be connected (if SignIn.RequireConfirmedEmail option is true)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        /// <summary>
        /// find an Identity user by his Id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(string userid);
        /// <summary>
        /// find an Identity user by his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetUserByUsernameAsync(string username);
        /// <summary>
        /// This will update EmailConfirmed field in Database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    }
}
