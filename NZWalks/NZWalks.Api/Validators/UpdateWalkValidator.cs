using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class UpdateWalkValidator:AbstractValidator<Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThanOrEqualTo(0);
        }
    }
}
