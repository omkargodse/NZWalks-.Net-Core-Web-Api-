using FluentValidation;
using NZWalks.Api.Models.DTO;

namespace NZWalks.Api.Validators
{
    public class CreateRegionValidator: AbstractValidator<AddRegionRequest>
    {
        public CreateRegionValidator()
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
