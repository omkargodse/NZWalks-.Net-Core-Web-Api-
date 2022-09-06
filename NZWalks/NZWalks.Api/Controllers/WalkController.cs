using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalk walk;
        private readonly IMapper mapper;

        public WalkController(IWalk walk,IMapper mapper)
        {
            this.walk = walk;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetWalks()
        {
            var walks = await walk.GetWalks();
            // convert to DTO 
            var walkDTO = mapper.Map<List<Models.DTO.Walk>>(walks);            
            
            if(walks == null) return NotFound();

            return Ok(walkDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkById")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            var walkDomain = await walk.GetWalkById(id);
            if (walkDomain == null) return NotFound();

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalk([FromBody] Models.DTO.AddWalkRequest newWalk)
        {
            // convert dto to domain 
            var walkDomain = new Models.Domains.Walk
            {
                Name = newWalk.Name,
                Length = newWalk.Length,
                RegionID = newWalk.RegionID,
                WalkDifficultyId = newWalk.WalkDifficultyId
            };

            //add to db 
            walkDomain = await walk.AddWalkAsync(walkDomain);

            // convert domain to dto 
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkById), new { id = walkDTO.Id},walkDTO);
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest walkRequest)
        {
            // DTO to domain 
            var walkDomain = new Models.Domains.Walk()
            {
                Name = walkRequest.Name,
                Length=walkRequest.Length,
                RegionID = walkRequest.RegionID,
                WalkDifficultyId = walkRequest.WalkDifficultyId
            };

            // add to db 
            walkDomain =await walk.UpdateWalkAsync(id,walkDomain);

            //null
            if (walkDomain == null) return NotFound();

            // domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id=walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var ExistingWalk = await walk.DeleteWalkAsync(id);
            if (ExistingWalk == null) return NotFound();

            // domain to dto
            var WalkDTO = new Models.DTO.Walk
            {
                Id = ExistingWalk.Id,
                Name = ExistingWalk.Name,
                Length = ExistingWalk.Length,
                RegionID = ExistingWalk.RegionID,
                WalkDifficultyId = ExistingWalk.WalkDifficultyId
            };
            
            return Ok(WalkDTO);
        }
    }
}
