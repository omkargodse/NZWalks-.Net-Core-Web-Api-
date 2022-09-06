using AutoMapper;

namespace NZWalks.Api.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile()
        {
            CreateMap<Models.Domains.Walk, Models.DTO.Walk>().ReverseMap();

            CreateMap<Models.Domains.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
        }
    }
}
