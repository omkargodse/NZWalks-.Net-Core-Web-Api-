using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class UpdateRegionValidator:AbstractValidator<Models.DTO.UpdateRegionRequest>
    {
        public UpdateRegionValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Lat).NotEmpty();
            RuleFor(x => x.Lat).NotEmpty();
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
