using AquaCulture.Infrastructure.Data;
using AquaCulture.Domain.Entities;
using AquaCulture.Application.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaCulture.Infrastructure.Repositories
{
    public class FishFarmRepository : Repository<FishFarm>, IFishFarmRepository
    {
        public FishFarmRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<FishFarm>> GetAllFarmsWithWorkersAsync()
        {
            return await _dbSet.Include(f => f.Workers).ToListAsync();
        }

        public async Task<FishFarm?> GetFarmWithWorkersAsync(Guid id)
        {
            return await _dbSet.Include(f => f.Workers).FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
