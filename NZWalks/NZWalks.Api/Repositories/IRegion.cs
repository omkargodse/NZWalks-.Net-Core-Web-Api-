using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public interface IRegion
    {
        public Task<IEnumerable<Region>> GetAllAsync();
    }
}
