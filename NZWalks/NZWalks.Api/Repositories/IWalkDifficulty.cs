using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
    public interface IWalkDifficulty
    {
        public Task<IEnumerable<WalkDifficulty>> GetWalkDifficulties();

        public Task<WalkDifficulty> GetWalkDiffsByID(Guid id);

        public Task<WalkDifficulty> CreateWalkDifficulty(WalkDifficulty walkDifficulty);

        public Task<WalkDifficulty> UpdateWalkDifficulty(Guid id,WalkDifficulty walkDifficulty);
        public Task<WalkDifficulty> DeleteWalkDifficulty(Guid id);
    }
}
