using AutoMapper;

namespace NZWalks.Api.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domains.Region, Models.DTO.Region>()
                .ReverseMap();
        }
    }
}
