using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Dto.Worker
{
    public class SearchWorkerDto
    {
        public string? SearchTerm { get; set; }
        public CrewRole? Position { get; set; }
        public string? SortBy { get; set; }
        public bool? IsAssigned { get; set; }
    }
}
