using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Models.DTO;
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
        public async Task<IActionResult> GetAllRegionAsync()
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

            var RegionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(RegionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionByIDAsync")]
        public async Task<IActionResult> GetRegionByIDAsync(Guid id)
        {
            var domainRegion = await this.region.GetAsync(id);
            if (domainRegion == null)
                return NotFound();

            var regionDTO = mapper.Map<Models.DTO.Region>(domainRegion);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion(AddRegionRequest addRegionRequest)
        {
            //Validate request 
            if(!ValidateRequestCreateRegion(addRegionRequest))
                return BadRequest(ModelState);

            // Convert incoming object to domain model / class
            var regionDomain = new Api.Models.Domains.Region
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            // save to Db via repository 
            regionDomain = await region.CreateAsync(regionDomain);

            //convert response to DTO
            var regionDTO = new Api.Models.DTO.Region()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                Area = regionDomain.Area,
                Lat = regionDomain.Lat,
                Long = regionDomain.Long,
                Population = regionDomain.Population
            };

            // first param tells at which action newly created resource can be found
            // second param is parameter to first param (action)
            // third param is the whole object 
            return CreatedAtAction(nameof(GetRegionByIDAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            // Get region from DB  
            var regionX = await region.DeleteAsync(id);

            // if null, not found
            if (regionX == null) return NotFound();

            // Convert response back to DTO 
            var regionDTO = new Models.Domains.Region
            {
                Code = regionX.Code,
                Name = regionX.Name,
                Area = regionX.Area,
                Lat = regionX.Lat,
                Long = regionX.Long,
                Population = regionX.Population
            }; ; 

            // Return ok response
            //await region.DeleteAsync(id);
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest regionX)
        {
            //Validate request
            //if (!ValidateUpdateRegion(regionX))
            //    return BadRequest(ModelState);

            //Convert DTO to domain  model
            var regionDomain = new Models.Domains.Region()
            {                
                Code = regionX.Code,
                Name = regionX.Name,
                Area = regionX.Area,
                Lat = regionX.Lat,
                Long = regionX.Long,
                Population = regionX.Population
            };

            //Update region using repository
            var updatedRegion = await region.UpdateAsync(id,regionDomain);

            // If null not found
            if(updatedRegion == null) return NotFound();

            // Convert domain back to DTO 
            var regionDTO = new Models.DTO.Region
            {
                Id = updatedRegion.Id,
                Code = updatedRegion.Code,
                Name = updatedRegion.Name,
                Area = updatedRegion.Area,
                Lat = updatedRegion.Lat,
                Long = updatedRegion.Long,
                Population = updatedRegion.Population
            };

            // Return Ok response 
            return Ok(regionDTO);
        }

        #region Private methods 
        private bool ValidateRequestCreateRegion(AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null) 
                ModelState.AddModelError(nameof(addRegionRequest),"Data is required");

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} Code cannot be null, empty or white space");

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} Code cannot be null, empty or white space");

            if (addRegionRequest.Area <=0)
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} Code cannot be less than or equal to zero");

            if (addRegionRequest.Lat <= 0)
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} Code cannot be less than or equal to zero");

            if (addRegionRequest.Long <= 0)
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} Code cannot be less than or equal to zero");

            if (addRegionRequest.Population < 0)
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} Code cannot be less than zero");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateRegion(Models.DTO.UpdateRegionRequest regionX)
        {
            if (regionX == null)
                ModelState.AddModelError(nameof(regionX), "Data is required");

            if (string.IsNullOrWhiteSpace(regionX.Code))
                ModelState.AddModelError(nameof(regionX.Code),
                    $"{nameof(regionX.Code)} Code cannot be null, empty or white space");

            if (string.IsNullOrWhiteSpace(regionX.Name))
                ModelState.AddModelError(nameof(regionX.Name),
                    $"{nameof(regionX.Name)} Code cannot be null, empty or white space");

            if (regionX.Area <= 0)
                ModelState.AddModelError(nameof(regionX.Area),
                    $"{nameof(regionX.Area)} Code cannot be less than or equal to zero");

            //if (addRegionRequest.Lat <= 0)
            //    ModelState.AddModelError(nameof(addRegionRequest.Lat),
            //        $"{nameof(addRegionRequest.Lat)} Code cannot be less than or equal to zero");

            //if (addRegionRequest.Long <= 0)
            //    ModelState.AddModelError(nameof(addRegionRequest.Long),
            //        $"{nameof(addRegionRequest.Long)} Code cannot be less than or equal to zero");

            if (regionX.Population < 0)
                ModelState.AddModelError(nameof(regionX.Population),
                    $"{nameof(regionX.Population)} Code cannot be less than zero");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
        #endregion
    }
}
