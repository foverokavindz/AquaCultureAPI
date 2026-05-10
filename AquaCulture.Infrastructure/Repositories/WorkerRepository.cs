using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.Interfaces.Repositories;
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

        public async Task<IEnumerable<Worker>> SearchWorkerAsync(SearchWorkerDto dto)
        {
            var query = _dbSet.Include(w => w.FishFarm).AsQueryable();

            // filter by name
            if (!string.IsNullOrEmpty(dto.SearchTerm))
                query = query.Where(w => w.Name.ToLower().Contains(dto.SearchTerm.ToLower()));

            // filter by age
            if (dto.Position.HasValue)
                query = query.Where(w => w.Position == dto.Position.Value);

            // filter unassigned workers
            if (dto.IsAssigned.HasValue)
            {
                if (dto.IsAssigned.Value)   
                    query = query.Where(w => w.FishFarmId != null); 
                else
                    query = query.Where(w => w.FishFarmId == null); 
            }

            // sorting
            query = dto.SortBy switch
            {
                "name_asc" => query.OrderBy(w => w.Name),
                "name_desc" => query.OrderByDescending(w => w.Name),
                "age_asc" => query.OrderBy(f => f.Age),
                "age_desc" => query.OrderByDescending(f => f.Age),
                _ => query.OrderBy(f => f.Name) 
            };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Worker>> GetAllWithFarmAsync()
        {
            return await _dbSet
                .Include(w => w.FishFarm)
                .ToListAsync();
        }

        public async Task<Worker?> GetByIdWithFishFarmAsync(Guid id)
        {
            return await _dbSet
                .Include(w => w.FishFarm)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }


}
