using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public interface IWalk
    {
        public Task<IEnumerable<Walk>> GetWalks();
        public Task<Walk> GetWalkById(Guid id);
        public Task<Walk> AddWalkAsync(Walk walk);
        public Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
        public Task<Walk> DeleteWalkAsync(Guid id);
    }
}
