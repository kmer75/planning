using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;

namespace PlanningApi.Validations
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(vm => vm.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Email.EmptyCode, ErrorContent.UserContext.Email.EmptyMessage))
                .EmailAddress().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Email.ValidAddressCode, ErrorContent.UserContext.Email.ValidAddressMessage));
        }
    }
}
