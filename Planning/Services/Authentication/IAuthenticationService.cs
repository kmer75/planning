using Microsoft.AspNetCore.Identity;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlanningApi.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// get current user claims
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> GetClaimsIdentity(string userName);
        /// <summary>
        /// it will verify if the user mail is confirmed, or is locked
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <param name="lockoutOnFailure"></param>
        Task<SignInResult> SignInAsync(string username, string password, bool rememberMe = true, bool lockoutOnFailure = false);
        /// <summary>
        /// Not used because we use JWT
        /// </summary>
        void SignOutAsync();
        /// <summary>
        /// Get the User by EMail
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Application User</returns>
        Task<User> FindByEmailAsync(string email);
        /// <summary>
        /// Get the User by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Application User</returns>
        Task<User> FindByIdAsync(string Id);
        /// <summary>
        /// Check if the email is already confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> IsEmailConfirmedAsync(User user);
        /// <summary>
        /// generate a token for forgotPassword
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetTokenAsync(User user);
        /// <summary>
        /// Reset the current password by the new password for a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    }
}
