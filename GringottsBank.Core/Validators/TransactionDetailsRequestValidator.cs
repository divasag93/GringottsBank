using FluentValidation;
using GringottsBank.DataContracts;
using System;

namespace GringottsBank.Core.Validators
{
    public class TransactionDetailsRequestValidator : AbstractValidator<TransactionDetailsRequest>
    {
        public TransactionDetailsRequestValidator()
        {
            RuleFor(txnRequest => txnRequest.AccountNumber)
                .NotNull()
                .WithErrorCode(Error.Code.MissingAccountNumber)
                .WithMessage(Error.Message.MissingAccountNumber)
                .NotEmpty()
                .WithErrorCode(Error.Code.MissingAccountNumber)
                .WithMessage(Error.Message.MissingAccountNumber)
                .SetValidator(new WholeNumberValidator("accountNumber"));

            When(txnRequest => txnRequest.From != null,
               () => RuleFor(txnRequest => txnRequest.From)
                       .Must(IsDate)
                       .WithErrorCode(Error.Code.InvalidFromDate)
                       .WithMessage(Error.Message.InvalidFromDate)
               );

            When(txnRequest => txnRequest.To != null,
                () => RuleFor(txnRequest => txnRequest.To)
                        .Must(IsDate)
                        .WithErrorCode(Error.Code.InvalidToDate)
                        .WithMessage(Error.Message.InvalidToDate)
                );
                
        }

        private bool IsDate(string date)
        {
            DateTime output;
            var isDate = DateTime.TryParse(date, out output);
            return isDate;
        }
    }
}
