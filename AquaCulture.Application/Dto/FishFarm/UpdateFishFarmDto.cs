using System.ComponentModel.DataAnnotations;

namespace AquaCulture.Application.Dto.FishFarm
{
    public class UpdateFishFarmDto
    {
        [MaxLength(200, ErrorMessage = "Farm name cannot exceed 200 characters")]
        public string? Name { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public decimal? Longitude { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of cages cannot be negative")]
        public int? NoOfCages { get; set; }

        public bool? HasBarge { get; set; }

        public string? PictureUrl { get; set; }

        public List<WorkerAssignmentDto>? Workers { get; set; } 
    }
}