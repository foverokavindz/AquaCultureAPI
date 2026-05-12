using AquaCulture.Application.Dto.FishFarm;

namespace AquaCulture.Application.Interfaces.Services
{
    public interface IFishFarmService
    {
        Task<IEnumerable<FishFarmDto>> GetAllFarmsAsync();
        Task<FishFarmDto> GetFishFarmByIdAsync(Guid id);
        Task<FishFarmDto> CreateFishFarmAsync(CreateFishFarmDto dto);
        Task<FishFarmDto> UpdateFishFarmAsync(Guid id, UpdateFishFarmDto dto);
        Task<bool> DeleteFishFarmAsync(Guid id);
        Task<IEnumerable<FishFarmDto>> SearchFishFarmsAsync(SearchFishFarmDto dto);
    }
}
