using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class CreateWalkDifficultyValidator:AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public CreateWalkDifficultyValidator()
        {
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
