using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public interface IRegion
    {
        public Task<IEnumerable<Region>> GetAllAsync();

        public Task<Region> GetAsync(Guid id);

        public Task<Region> CreateAsync(Region region); 

        public Task<Region> DeleteAsync(Guid id);

        public Task<Region> UpdateAsync(Guid id,Region region);
    }
}
