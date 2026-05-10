namespace AquaCulture.Application.DTOs.FishFarm
{
    public class FishFarmSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
    }
}