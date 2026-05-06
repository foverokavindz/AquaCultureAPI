namespace AquaCulture.Domain.Entities
{
    public class Worker
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public required int Age { get; set; }
        public required string Email { get; set; }
        public required CrewRole Position { get; set; }
        public required DateOnly CertifiedUntil { get; set; }
        public Guid FishFarmId { get; set; }
        public FishFarm FishFarm { get; set; } = null!;
    }
}
