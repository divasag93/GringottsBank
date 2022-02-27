using FluentValidation;

namespace GringottsBank.Core.Validators
{
    public class WholeNumberValidator : AbstractValidator<string>
    {
        public WholeNumberValidator(string propertyName)
        {
            RuleFor(inp => inp)
                .Must(IsWholeNumber)
                .WithErrorCode(Error.Code.NotWholeNumber)
                .WithMessage(Error.Message.NotWholeNumber(propertyName));
        }
        private bool IsWholeNumber(string value)
        {
            float number = 0;
            var isWholeNumber = float.TryParse(value, out number);
            return isWholeNumber;
        }
    }
}
