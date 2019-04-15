using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;

namespace PlanningApi.Validations
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(vm => vm.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.UserName.EmptyCode, ErrorContent.UserContext.UserName.EmptyMessage));
            RuleFor(vm => vm.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Password.EmptyCode, ErrorContent.UserContext.Password.EmptyMessage));
        }
    }
}
