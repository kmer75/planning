using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;

namespace PlanningApi.Validations
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(vm => vm.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Password.EmptyCode, ErrorContent.UserContext.Password.EmptyMessage))
                .MinimumLength(6).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Password.LengthCode, ErrorContent.UserContext.Password.LengthMessage));
            RuleFor(vm => vm.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Equal(vm => vm.Password).WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.ConfirmPassword.EqualPasswordCode, ErrorContent.UserContext.ConfirmPassword.EqualPasswordMessage));
        }
    }
}
