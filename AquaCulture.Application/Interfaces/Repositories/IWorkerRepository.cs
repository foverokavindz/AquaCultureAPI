using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Interfaces.Repositories
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        public Task<IEnumerable<Worker>> GetWorkersByFarmIdAsync(Guid farmId);
        public Task<IEnumerable<Worker>> SearchWorkerAsync(SearchWorkerDto dto);

    }
}
