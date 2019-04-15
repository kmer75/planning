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
using PlanningApi.ViewModel;
using PlanningApi.Auth;
using PlanningApi.Helpers;
using PlanningApi.Services;
using Microsoft.AspNetCore.Authorization;
using static PlanningApi.Helpers.Constants;
using Repositories.Model;

namespace PlanningApi.Controllers
{
    /// <summary>
    /// controller for Authentication Section
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ClaimsPrincipal _caller;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Controller with the Depedency Injections
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="jwtFactory"></param>
        /// <param name="jwtOptions"></param>
        /// <param name="httpContextAccessor">the current context (can get the current Principal)</param>
        public AuthController(IAuthenticationService authService, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _authService = authService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _caller = httpContextAccessor.HttpContext.User;
            _emailSender = emailSender;
        }

        // POST api/auth/login
        /// <summary>
        ///  Sign in a user by his username and password.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>jwt Token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel credentials)
        {

            //sign in here ->
            var result =  await this._authService.SignInAsync(credentials.Username, credentials.Password, rememberMe : true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //Get identity for the current user credentials (Id and Roles)
                var identity = await _authService.GetClaimsIdentity(credentials.Username);
                
                //Generate Jwt Token with the specific Claims that we put in (Id, username, Roles, expire Date...)
                var jwt = await Tokens.GenerateJwt(identity,  _jwtFactory, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                return new OkObjectResult(jwt);
            }

            //Email not confirmed
            if(result.IsNotAllowed)
            {
                ModelState.AddModelError("Email", Helpers.Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.EmailNotConfirmed.EmailNotConfirmedCode, ErrorContent.UserContext.EmailNotConfirmed.EmailNotConfirmedMessage));
                return BadRequest(Errors.ErrorBuilder(this.ModelState));
            }

            return BadRequest(result);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            var user = await this._authService.FindByEmailAsync(model.Email);
            if (user == null || !(await this._authService.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                ModelState.AddModelError("Email", Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.User.UserNotFoundCode, ErrorContent.UserContext.User.UserNotFoundMessage));
                return BadRequest(Helpers.Errors.ErrorBuilder(this.ModelState));
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await this._authService.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = new Uri(Url.Link("ResetPasswordRoute", new { UserId = user.Id, Code = code }));

            try { 
            this._emailSender.SendEmail("Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", user.Email);
            } catch(Exception ex)
            {
                ModelState.AddModelError("Email", Helpers.Errors.ErrorMessageBuilderWithCode(ErrorContent.EmailSenderContext.Sender.SendCode, ErrorContent.EmailSenderContext.Sender.SendMessage));
                return BadRequest(Errors.ErrorBuilder(this.ModelState));
            }

            return new OkResult();
        }

        /// <summary>
        /// will display a static page to fill reset password formular
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ResetPassword", Name = "ResetPasswordRoute")]
        public async Task<ActionResult> ResetPassword(string code)
        {
            if (code == null)
            {
                return BadRequest("code null");
            }
            return new OkResult();
        }

        /// <summary>
        /// Reset the password
        /// </summary>
        /// <param name="model">password and confirm password</param>
        /// <param name="QsModel">User Id from the email link (query string parameter) and Token from the email link (query string parameter)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordViewModel model, [FromQuery]ResetPasswordFQViewModel QsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await this._authService.FindByIdAsync(QsModel.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError("UserId", Helpers.Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.User.UserNotFoundCode, ErrorContent.UserContext.User.UserNotFoundMessage));
                return BadRequest(Errors.ErrorBuilder(this.ModelState));
            }
            var result = await this._authService.ResetPasswordAsync(user, QsModel.Code, model.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult("Password updated");
            }
            return BadRequest(result);
        }


        /// <summary>
        /// the token has to be deleted front side
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "API_ACCESS")]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            this._authService.SignOutAsync();
            return new OkObjectResult("User logged out");
        }
    }

}
