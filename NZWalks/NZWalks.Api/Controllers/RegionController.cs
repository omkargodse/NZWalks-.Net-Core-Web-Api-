using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegion region;
        private readonly IMapper mapper;

        public RegionController(IRegion region, IMapper mapper)
        {
            this.region = region;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegion()
        {
            var regions = await this.region.GetAllAsync();

            // return DTO regions 

            //var RegionsDTO = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(region =>
            //{
            //    var RegionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,  
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    RegionsDTO.Add(RegionDTO);
            //});

            var RegionsDTO = mapper.Map <List<Models.DTO.Region>>(regions);

            return Ok(RegionsDTO);
        }
    }
}
