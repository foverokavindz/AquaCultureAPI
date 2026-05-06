using AquaCulture.Application.Dto.FishFarm;

namespace AquaCulture.Application.Interfaces
{
    public interface IFishFarmService
    {
        Task<IEnumerable<FishFarmDto>> GetAllFarmsAsync();
        Task<FishFarmDto> GetFarmByIdAsync(Guid id);
        Task<FishFarmDto> CreateFarmAsync(CreateFishFarmDto dto);
        Task<FishFarmDto> UpdateFarmAsync(Guid id, UpdateFishFarmDto dto);
        Task DeleteFarmAsync(Guid id);
    }
}
