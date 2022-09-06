using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficulty
    {
        private readonly NZWalksDbContext dbContext;

        public WalkDifficultyRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<WalkDifficulty> CreateWalkDifficulty(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await dbContext.WalkDifficulties.AddAsync(walkDifficulty);
            await dbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficulty = await dbContext.WalkDifficulties.FirstOrDefaultAsync(x=>x.Id==id);
            if (walkDifficulty == null) return null;

            dbContext.WalkDifficulties.Remove(walkDifficulty);
            await dbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetWalkDifficulties()
        {
            return await dbContext.WalkDifficulties.ToListAsync();                         
        }

        public async Task<WalkDifficulty> GetWalkDiffsByID(Guid id)
        {
            var WalkDifficulty = await dbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if(WalkDifficulty == null) return null;
            return WalkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateWalkDifficulty(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkD = await dbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalkD == null) return null;

            existingWalkD.Code = walkDifficulty.Code;
            await dbContext.SaveChangesAsync();
            return existingWalkD;
        }
    }
}
