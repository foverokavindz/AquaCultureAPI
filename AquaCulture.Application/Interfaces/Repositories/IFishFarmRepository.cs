using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Interfaces.Repositories
{
    public interface IFishFarmRepository : IRepository<FishFarm>
    {
        Task<FishFarm> GetFishFarmByIdWithWorkersAsync(Guid id);
        Task<IEnumerable<FishFarm>> GetAllFarmsWithWorkersAsync();
        Task<IEnumerable<FishFarm>> SearchFarmsAsync(SearchFishFarmDto dto);
        
    }
}
