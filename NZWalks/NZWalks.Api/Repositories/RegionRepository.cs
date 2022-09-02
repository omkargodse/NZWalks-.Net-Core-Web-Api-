using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public class RegionRepository : IRegion
    {
        private readonly NZWalksDbContext dbContext;
        public RegionRepository(NZWalksDbContext context)
        {
            dbContext = context;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }
    }
}
