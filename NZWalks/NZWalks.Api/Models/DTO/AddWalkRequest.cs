﻿namespace NZWalks.Api.Models.DTO
{
    public class AddWalkRequest
    {        
        public string Name { get; set; }

        public double Length { get; set; }
        public Guid RegionID { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}
