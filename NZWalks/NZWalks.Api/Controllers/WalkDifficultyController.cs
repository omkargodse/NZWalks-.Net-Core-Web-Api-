using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("controller")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficulty walkDifficultyService;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficulty walkDifficulty, IMapper mapper)
        {
            this.walkDifficultyService = walkDifficulty;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultyDomain = await walkDifficultyService.GetWalkDifficulties();

            if (walkDifficultyDomain == null)
                return NotFound();

            var walkDifficultyDTO = new List<Models.DTO.WalkDifficulty>();

            walkDifficultyDomain.ToList().ForEach(walk =>
            {
                var walkDifficulty = new Models.DTO.WalkDifficulty()
                {
                    Id = walk.Id,
                    Code = walk.Code,
                };
                walkDifficultyDTO.Add(walkDifficulty);
            });
            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDomain = await walkDifficultyService.GetWalkDiffsByID(id);
            if (walkDomain == null)
                return NotFound();

            //domain to dto
            var walkDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDomain.Id,
                Code = walkDomain.Code,
            };

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalkDifficulty(Models.DTO.AddWalkDifficultyRequest walkDifficulty)
        {
            //validateRequest 
            if (!ValidateCreateWalkDifficulty(walkDifficulty))
                return BadRequest(ModelState);
            //dto to domain
            var walkDomain = new Models.Domains.WalkDifficulty()
            {
                Code = walkDifficulty.Code,
            };
            walkDomain = await walkDifficultyService.CreateWalkDifficulty(walkDomain);

            //domain to dto 
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate Request
            if (!ValidateUpdateWalkDifficulty(updateWalkDifficultyRequest))
                return BadRequest(ModelState);
            //dto to domain 
            var walkDomain = new Models.Domains.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //add to db 
            walkDomain = await walkDifficultyService.UpdateWalkDifficulty(id, walkDomain);
            if (walkDomain == null) return NotFound();

            //domain to dto
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty([FromRoute] Guid id)
        {
            var walkDomain = await walkDifficultyService.DeleteWalkDifficulty(id);
            if (walkDomain == null) return NotFound();

            //domain to dto 
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);
            return Ok(walkDTO);
        }

        #region Private methods
        private bool ValidateCreateWalkDifficulty(Models.DTO.AddWalkDifficultyRequest walkDifficulty)
        {
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(walkDifficulty), $"{nameof(walkDifficulty)} cannot be null");

            if (string.IsNullOrWhiteSpace(walkDifficulty.Code))
                ModelState.AddModelError(nameof(walkDifficulty.Code), $"{nameof(walkDifficulty.Code)} cannot be null, empty");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateWalkDifficulty(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"{nameof(updateWalkDifficultyRequest)} cannot be null");

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null, empty");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
        #endregion
    }
}
