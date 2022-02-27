using FluentValidation;
using GringottsBank.DataContracts;

namespace GringottsBank.Core.Validators
{
    public class NameValidator : AbstractValidator<Name>
    {
        public NameValidator()
        {
            RuleFor(name => name.First)
                .Cascade(CascadeMode.Continue)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(Error.Code.FirstNameRequired)
                .WithMessage(Error.Message.FirstNameRequired);

            RuleFor(name => name.Last)
                .Cascade(CascadeMode.Continue)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(Error.Code.LastNameRequired)
                .WithMessage(Error.Message.LastNameRequired);
        }
    }
}
