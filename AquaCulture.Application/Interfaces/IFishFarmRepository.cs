using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.interfaces
{
    public interface IFishFarmRepository : IRepository<FishFarm>
    {
        Task<FishFarm?> GetFarmWithWorkersAsync(Guid id);
        Task<IEnumerable<FishFarm>> GetAllFarmsWithWorkersAsync();
    }
}
