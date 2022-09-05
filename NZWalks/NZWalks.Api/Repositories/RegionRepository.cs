using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace NZWalks.Api.Repositories
{
    public class RegionRepository : IRegion
    {
        private readonly NZWalksDbContext dbContext;
        public RegionRepository(NZWalksDbContext context)
        {
            dbContext = context;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await dbContext.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null) return null;

            dbContext.Regions.Remove(region);

            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id,Region region)
        {
            // get from db
            var regionToUpdate = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // return null if not found
            if(regionToUpdate==null)
            {
                return null;
            }

            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.Area = region.Area;
            regionToUpdate.Lat = region.Lat;
            regionToUpdate.Long = region.Long;
            regionToUpdate.Population = region.Population;

            await dbContext.SaveChangesAsync();
            return regionToUpdate;
        }
    }
}
