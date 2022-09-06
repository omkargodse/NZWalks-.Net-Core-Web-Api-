using AutoMapper;

namespace NZWalks.Api.Profiles
{
    public class WalkDifficultyProfile:Profile
    {
        public WalkDifficultyProfile()
        {
            CreateMap<Models.Domains.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
        }
    }
}
