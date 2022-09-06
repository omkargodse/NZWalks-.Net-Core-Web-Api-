namespace NZWalks.Api.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double Length { get; set; }
        public Guid RegionID { get; set; }
        public Guid WalkDifficultyId { get; set; }

        // Navigation property
         
        public Region Region { get; set; } // 1:1 relationship between Walk and Region
        public WalkDifficulty WalkDifficulty { get; set; } // 1:1 relationship between Walk and WalkDifficulty
    }
}
