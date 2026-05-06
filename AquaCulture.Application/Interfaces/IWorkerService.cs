using AquaCulture.Application.Dto.Worker;

namespace AquaCulture.Application.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<WorkerDto>> GetWorkersByFarmIdAsync(Guid farmId);
        Task<WorkerDto> GetWorkerByIdAsync(Guid id);
        Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto dto);
        Task<WorkerDto> UpdateWorkerAsync(Guid id, UpdateWorkerDto dto);
        Task DeleteWorkerAsync(Guid id);
    }
}
