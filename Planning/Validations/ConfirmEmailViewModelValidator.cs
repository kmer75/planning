using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningApi.ViewModel;
using PlanningApi.Helpers;

namespace PlanningApi.Validations
{
    public class ConfirmEmailViewModelValidator : AbstractValidator<ConfirmEmailViewModel>
    {
        public ConfirmEmailViewModelValidator()
        {
            RuleFor(vm => vm.Code)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.Code.EmptyTokenCode, ErrorContent.UserContext.Code.EmptyTokenMessage));
            RuleFor(vm => vm.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(Errors.ErrorMessageBuilderWithCode(ErrorContent.UserContext.UserId.EmptyUserIdCode, ErrorContent.UserContext.UserId.EmptyUserIdMessage));
        }
    }
}
