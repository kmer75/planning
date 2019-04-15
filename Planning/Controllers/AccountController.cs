using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;
using AutoMapper;
using PlanningApi.Services;
using Repositories.Repositories;
using Repositories.Model;

namespace PlanningApi.Controllers
{
    /// <summary>
    /// controller for the Account section
    /// </summary>
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class AccountController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller with the Depedency Injections
        /// </summary>
        /// <param name="registerService"></param>
        /// <param name="emailSender"></param>
        /// <param name="mapper"></param>
        public AccountController(
                                 IRegisterService registerService,
                                 IEmailSender emailSender,
                                 IMapper mapper
            )
        {
            _registerService = registerService;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        /// <summary>
        /// register a new user
        /// </summary>
        /// <param name="model">object that register a user</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            ModelToVmMapper.BuidUserFromRegisterVm(user);

            var result = await this._registerService.CreateUserIdentityAsync(user, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await this._registerService.SetRoleToUserAsync(user.UserName, new List<string>() { "USER" });

            //email confirm
            var code = await this._registerService.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { UserId = user.Id, Code = code }));

            try { 
            this._emailSender.SendEmail("Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>", user.Email);
            } catch(Exception ex)
            {
                ModelState.AddModelError("Email", Helpers.Errors.ErrorMessageBuilderWithCode(ErrorContent.EmailSenderContext.Sender.SendCode, ErrorContent.EmailSenderContext.Sender.SendMessage));
                return BadRequest(Errors.ErrorBuilder(this.ModelState));
            }

            return new OkObjectResult("Account created");
        }

        /// <summary>
        /// Confirm user registeration email after validating an email 
        /// </summary>
        /// <param name="confirmEmailVm">user id to retrieve the user and token for the validation</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]ConfirmEmailViewModel confirmEmailVm)
        {
            if (string.IsNullOrWhiteSpace(confirmEmailVm.UserId) || string.IsNullOrWhiteSpace(confirmEmailVm.Code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var user = await this._registerService.GetUserByIdAsync(confirmEmailVm.UserId);

            IdentityResult result = await this._registerService.ConfirmEmailAsync(user, confirmEmailVm.Code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("An error occured");
            }
        }



    }
}
