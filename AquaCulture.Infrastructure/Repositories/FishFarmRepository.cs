using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Interfaces.Repositories;
using AquaCulture.Domain.Entities;
using AquaCulture.Infrastructure.Data;
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

        public async Task<FishFarm?> GetFishFarmByIdWithWorkersAsync(Guid id)
        {
            return await _dbSet.Include(f => f.Workers).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FishFarm>> SearchFarmsAsync(SearchFishFarmDto dto)
        {
            var query = _dbSet.Include(f => f.Workers).AsQueryable();

            // filter by name
            if (!string.IsNullOrEmpty(dto.SearchTerm))
                query = query.Where(f => f.Name.ToLower().Contains(dto.SearchTerm.ToLower()));

            // filter by barge
            if (dto.HasBarge.HasValue)
                query = query.Where(f => f.HasBarge == dto.HasBarge.Value);

            // filter by cage range
            if (dto.MinAvailableCages.HasValue)
                query = query.Where(f => f.NoOfCages >= dto.MinAvailableCages.Value);

            if (dto.MaxAvailableCages.HasValue)
                query = query.Where(f => f.NoOfCages <= dto.MaxAvailableCages.Value);

            // sorting
            query = dto.SortBy switch
            {
                "name_asc" => query.OrderBy(f => f.Name),
                "name_desc" => query.OrderByDescending(f => f.Name),
                "cages_asc" => query.OrderBy(f => f.NoOfCages),
                "cages_desc" => query.OrderByDescending(f => f.NoOfCages),
                _ => query.OrderBy(f => f.Name) // default
            };

            return await query.ToListAsync();
        }
    }
}
