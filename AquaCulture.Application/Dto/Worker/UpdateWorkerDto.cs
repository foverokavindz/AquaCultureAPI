using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Dto.Worker
{
    public class UpdateWorkerDto
    {
        public string? Name { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public CrewRole? Position { get; set; }
        public DateOnly? CertifiedUntil { get; set; }
    }
}
