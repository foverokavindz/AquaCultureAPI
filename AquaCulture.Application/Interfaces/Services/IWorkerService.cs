using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Interfaces.services
{
    public interface IWorkerService
    {
        Task<IEnumerable<WorkerDto>> GetWorkersByFishFarmIdAsync(Guid farmId);
        Task<IEnumerable<WorkerDto>> GetAllWorkersAsync();
        Task<WorkerDto> GetWorkerByIdAsync(Guid id);
        Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto dto);
        Task<WorkerDto> UpdateWorkerAsync(Guid id, UpdateWorkerDto dto);
        Task DeleteWorkerAsync(Guid id);
        Task<IEnumerable<WorkerDto>> SearchWorkerAsync(SearchWorkerDto dto);
        Task<WorkerDto?> GetByIdWithFishFarmAsync(Guid id);
    }
}
