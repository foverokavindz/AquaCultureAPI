using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.interfaces
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        public Task<IEnumerable<Worker>> GetWorkersByFarmIdAsync(Guid farmId);

    }
}
