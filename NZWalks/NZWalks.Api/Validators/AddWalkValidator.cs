using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class AddWalkValidator:AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x=>x.Length).GreaterThanOrEqualTo(0);            
        }
    }
}
