using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public class WalkRepository : IWalk
    {
        private readonly NZWalksDbContext dbContext;

        public WalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteWalkAsync(Guid id)
        {
            var walk = dbContext.Walks.FirstOrDefault(x => x.Id == id);
            if (walk == null) return null;
            
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> GetWalkById(Guid id)
        {
            var WalkDomain = await dbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);


            if(WalkDomain == null) return null;
            return WalkDomain;
        }

        public async Task<IEnumerable<Walk>> GetWalks()
        {
            return await dbContext.Walks
                .Include(x=>x.Region) // this includes Region navigation property, if we skip this, the value will be null
                .Include(x=>x.WalkDifficulty) // this includes WalkDifficulty navigation property, if we skip this, the value will be null
                .ToListAsync();
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var walkDomain =  await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomain == null) return null;
            // domain to dto 
            walkDomain.Length = walk.Length;
            walkDomain.Name = walk.Name;
            walkDomain.RegionID = walk.RegionID;
            walkDomain.WalkDifficultyId = walk.WalkDifficultyId;

            await dbContext.SaveChangesAsync();
            return walkDomain;
        }
    }
}
