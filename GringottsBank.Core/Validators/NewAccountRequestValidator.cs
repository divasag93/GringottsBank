using FluentValidation;
using GringottsBank.DataContracts;

namespace GringottsBank.Core.Validators
{
    public class NewAccountRequestValidator : AbstractValidator<NewAccountRequest>
    {
        public NewAccountRequestValidator()
        {
            RuleFor(accReq => accReq.CustomerId)
                .NotNull()
                .WithErrorCode(Error.Code.MissingCustomerID).WithMessage(Error.Message.MissingCustomerID)
                .NotEmpty()
                .WithErrorCode(Error.Code.MissingCustomerID).WithMessage(Error.Message.MissingCustomerID);

            RuleFor(accReq => accReq.InitialDeposit)
                .GreaterThan(0)
                .WithErrorCode(Error.Code.InvalidTransactionAmount).WithMessage(Error.Message.InvalidTransactionAmount);
        }
    }
}
