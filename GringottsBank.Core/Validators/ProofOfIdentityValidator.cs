using FluentValidation;
using GringottsBank.DataContracts;

namespace GringottsBank.Core.Validators
{
    public class ProofOfIdentityValidator : AbstractValidator<IdentityProof>
    { 
        public ProofOfIdentityValidator()
        {
            RuleFor(proof => proof.Proof)
                .NotNull()
                .WithErrorCode(Error.Code.MissingIdentityProof)
                .WithMessage(Error.Message.MissingIdentityProof)
                .NotEmpty()
                .WithErrorCode(Error.Code.MissingIdentityProof)
                .WithMessage(Error.Message.MissingIdentityProof);

            RuleFor(proof => proof.Name)
                .NotNull()
                .WithErrorCode(Error.Code.MissingIdentityProof)
                .WithMessage(Error.Message.MissingIdentityProof)
                .NotEmpty()
                .WithErrorCode(Error.Code.MissingIdentityProof)
                .WithMessage(Error.Message.MissingIdentityProof);
        }
    }
}
