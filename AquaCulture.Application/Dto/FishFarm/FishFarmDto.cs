using AquaCulture.Application.Dto.Worker;

namespace AquaCulture.Application.Dto.FishFarm
{
    public class FishFarmDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int NoOfCages { get; set; }
        public bool HasBarge { get; set; }
        public string PictureUrl { get; set; }
        public List<WorkerDto> Workers { get; set; } = [];
    }
}
