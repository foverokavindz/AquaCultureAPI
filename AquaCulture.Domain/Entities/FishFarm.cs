namespace AquaCulture.Domain.Entities
{
    public class FishFarm
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required GeoLocation GpsLocation { get; set; }
        public required int NoOfCages { get; set; }
        public required bool HasBarge { get; set; }
        public required string PictureUrl { get; set; }
        public List<Worker> Workers { get; set; } = [];

        // is deleted flag for soft delete
    }
}
