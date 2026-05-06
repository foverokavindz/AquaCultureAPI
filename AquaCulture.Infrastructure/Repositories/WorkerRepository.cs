using AquaCulture.Application.interfaces;
using AquaCulture.Domain.Entities;
using AquaCulture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaCulture.Infrastructure.Repositories
{
    public class WorkerRepository : Repository<Worker>, IWorkerRepository
    {
        public WorkerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Worker>> GetWorkersByFarmIdAsync(Guid farmId)
        {
            return await _dbSet.Where(w => w.FishFarmId == farmId).ToListAsync();
        }
    }
}
