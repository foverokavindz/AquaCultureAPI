using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Dto.FishFarm
{
    public class WorkerAssignmentDto
    {
        public Guid Id { get; set; }
        public CrewRole? Position { get; set; }

    }
}
