using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;
using PlanningApi.Services;
using Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Repositories.Model;

namespace PlanningApi.Validations
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        private IUserService _service;
        
        public RegisterViewModelValidator(IUserService service)
        {
            _service = service;

            RuleFor(vm => vm.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Password.EmptyCode, ErrorContent.UserContext.Password.EmptyMessage))
                .Length(6, 12).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Password.LengthCode, ErrorContent.UserContext.Password.LengthMessage));
            RuleFor(vm => vm.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Equal(vm => vm.Password).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.ConfirmPassword.EqualPasswordCode, ErrorContent.UserContext.ConfirmPassword.EqualPasswordMessage));
            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.FirstName.EmptyCode, ErrorContent.UserContext.FirstName.EmptyMessage))
                .MinimumLength(2).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.FirstName.MinLengthCode, ErrorContent.UserContext.FirstName.MinLengthMessage));
            RuleFor(u => u.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.UserName.EmptyCode, ErrorContent.UserContext.UserName.EmptyMessage))
                .MinimumLength(2).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.UserName.MinLengthCode, ErrorContent.UserContext.UserName.MinLengthMessage))
                .Must(s => !this._service.IsUserNameExist(s)).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.UserName.ExistCode, ErrorContent.UserContext.UserName.ExistMessage));
            RuleFor(u => u.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.FirstName.EmptyCode, ErrorContent.UserContext.FirstName.EmptyMessage))
                .MinimumLength(2).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.LastName.MinLengthCode, ErrorContent.UserContext.LastName.MinLengthMessage));
            RuleFor(u => u.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Email.EmptyCode, ErrorContent.UserContext.Email.EmptyMessage));
            RuleFor(u => u.Email).EmailAddress().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Email.ValidAddressCode, ErrorContent.UserContext.Email.ValidAddressMessage));
            RuleFor(u => u.Sex).Must(s => s == Constants.Sex.Male || s == Constants.Sex.Female).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Sex.MustCode, ErrorContent.UserContext.Sex.MustMessage));
            RuleFor(u => u.DateOfBirth)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.DateOfBirth.EmptyCode, ErrorContent.UserContext.DateOfBirth.EmptyMessage))
                .Matches(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$").WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.DateOfBirth.RegexCode, ErrorContent.UserContext.DateOfBirth.RegexMessage));
           }

        
    }
}

