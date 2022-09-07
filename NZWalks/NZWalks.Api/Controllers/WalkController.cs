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
        private readonly IRegion regionService;
        private readonly IWalkDifficulty walkDifficultyService;

        public WalkController(IWalk walk,IMapper mapper, IRegion region, IWalkDifficulty walkDifficulty)
        {
            this.walk = walk;
            this.mapper = mapper;
            this.regionService = region;
            this.walkDifficultyService = walkDifficulty;
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
            //validate request 
            if (!(await ValidateAddWalk(newWalk)))
                return BadRequest(ModelState);
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
            //Validate request 
            if (!(await ValidateUpdateWalkAsync(walkRequest)))
                return BadRequest(ModelState);
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

        #region Private methods
        private async Task<bool> ValidateAddWalk(Models.DTO.AddWalkRequest newWalk)
        {
            if(newWalk == null) 
                ModelState.AddModelError(nameof(newWalk),$"{nameof(newWalk)} Walk data is required");

            if (string.IsNullOrWhiteSpace(newWalk.Name))
                ModelState.AddModelError(nameof(newWalk.Name), $"{nameof(newWalk.Name)} can't be empty, null");

            if (newWalk.Length <= 0)
                ModelState.AddModelError(nameof(newWalk.Length),$"{nameof(newWalk.Length)} can't be null, zero or empty");
            
            var region = await regionService.GetAsync(newWalk.RegionID);
            if (region==null)
                ModelState.AddModelError(nameof(newWalk.RegionID), $"{nameof(newWalk.RegionID)} is invalid");

            var walkDiff = await walkDifficultyService.GetWalkDiffsByID(newWalk.WalkDifficultyId);
            if (walkDiff == null)
                ModelState.AddModelError(nameof(newWalk.WalkDifficultyId), $"{nameof(newWalk.WalkDifficultyId)} is invalid");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest walkRequest)
        {
            if (walkRequest == null)
                ModelState.AddModelError(nameof(walkRequest), $"{nameof(walkRequest)} cannot be empty");

            if (string.IsNullOrWhiteSpace(walkRequest.Name))
                ModelState.AddModelError(nameof(walkRequest.Name), $"{nameof(walkRequest.Name)} can't be empty, null");

            if (walkRequest.Length <= 0)
                ModelState.AddModelError(nameof(walkRequest.Length), $"{nameof(walkRequest.Length)} can't be null, zero or empty");

            var region = regionService.GetAsync(walkRequest.RegionID);
            if (region == null)
                ModelState.AddModelError(nameof(walkRequest.RegionID), $"{nameof(walkRequest.RegionID)} is invalid");

            var walkDiff = walkDifficultyService.GetWalkDiffsByID(walkRequest.WalkDifficultyId);
            if (walkDiff == null)
                ModelState.AddModelError(nameof(walkRequest.WalkDifficultyId), $"{nameof(walkRequest.WalkDifficultyId)} is invalid");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
        #endregion
    }
}
