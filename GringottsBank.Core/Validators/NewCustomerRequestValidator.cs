using FluentValidation;
using GringottsBank.DataContracts;
using System;

namespace GringottsBank.Core.Validators
{
    public class NewCustomerRequestValidator : AbstractValidator<NewCustomerRequest>
    {
        public NewCustomerRequestValidator()
        {
            RuleFor(request => request.CustomerName)
                .NotNull()
                .WithErrorCode(Error.Code.NameRequired)
                .WithMessage(Error.Message.NameRequired)
                .NotEmpty()
                .WithErrorCode(Error.Code.NameRequired)
                .WithMessage(Error.Message.NameRequired)
                .SetValidator(new NameValidator());

            RuleFor(request => request.DateOfBirth)
                .NotNull()
                .WithErrorCode(Error.Code.MissingDateOfBirth)
                .WithMessage(Error.Message.MissingDateOfBirth)
                .NotEmpty()
                .WithErrorCode(Error.Code.MissingDateOfBirth)
                .WithMessage(Error.Message.MissingDateOfBirth)
                .Must(IsDate)
                .WithErrorCode(Error.Code.InvalidDOB)
                .WithMessage(Error.Message.InvalidDOB)
                .Must(IsEligible)
                .WithErrorCode(Error.Code.AgeConstraint)
                .WithMessage(Error.Message.AgeConstraint);

            RuleFor(request => request.ProofOfIdentity)
                .NotNull()
                .WithErrorCode(Error.Code.MissingIdentityProof)
                .WithMessage(Error.Message.MissingIdentityProof)
                .SetValidator(new ProofOfIdentityValidator());
        }

        private bool IsDate(string date)
        {
            DateTime output;
            var isDate = DateTime.TryParse(date, out output);
            return isDate;
        }

        private bool IsEligible(string date)
        {
            DateTime output;
            DateTime.TryParse(date, out output);
            var age = DateTime.Now.Year - output.Year - 1;
            return age >= 15;
        }
    }
}
