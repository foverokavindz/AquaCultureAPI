using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.interfaces;
using AquaCulture.Application.Interfaces;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Services
{
    public class FishFarmService : IFishFarmService
    {
        private readonly IFishFarmRepository _farmRepository;

        public FishFarmService(IFishFarmRepository farmRepository)
        {
            _farmRepository = farmRepository;
        }

        public async Task<IEnumerable<FishFarmDto>> GetAllFarmsAsync()
        {
            var farms = await _farmRepository.GetAllFarmsWithWorkersAsync();

            return farms.Select(f => new FishFarmDto
            {
                Id = f.Id,
                Name = f.Name,
                Latitude = f.GpsLocation.Latitude,
                Longitude = f.GpsLocation.Longitude,
                NoOfCages = f.NoOfCages,
                HasBarge = f.HasBarge,
                PictureUrl = f.PictureUrl,
                Workers = f.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    ProfileImageUrl = w.ProfileImageUrl,
                    Age = w.Age,
                    Email = w.Email,
                    CertifiedUntil = w.CertifiedUntil,
                    FishFarmId = w.FishFarmId,
                    Position = w.Position,
                }).ToList()
            });
        }

        public async Task<FishFarmDto> GetFarmByIdAsync(Guid id)
        {
            var farm = await _farmRepository.GetFarmWithWorkersAsync(id);
            if (farm == null)
                throw new KeyNotFoundException($"Fish farm with ID {id} not found.");

            return new FishFarmDto
            {
                Id = farm.Id,
                Name = farm.Name,
                Latitude = farm.GpsLocation.Latitude,
                Longitude = farm.GpsLocation.Longitude,
                NoOfCages = farm.NoOfCages,
                HasBarge = farm.HasBarge,
                PictureUrl = farm.PictureUrl,
                Workers = farm.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    ProfileImageUrl = w.ProfileImageUrl,
                    Age = w.Age,
                    Email = w.Email,
                    CertifiedUntil = w.CertifiedUntil,
                    FishFarmId = w.FishFarmId,
                    Position = w.Position,
                }).ToList()
            };
        }

        public async Task<FishFarmDto> CreateFarmAsync(CreateFishFarmDto dto)
        {
            var farm = new FishFarm
            {
                Name = dto.Name,
                GpsLocation = new GeoLocation(dto.Latitude, dto.Longitude),
                NoOfCages = dto.NoOfCages,
                HasBarge = dto.HasBarge,
                PictureUrl = dto.PictureUrl
            };
            await _farmRepository.AddAsync(farm);
            await _farmRepository.SaveChangesAsync();
            return new FishFarmDto
            {
                Id = farm.Id,
                Name = farm.Name,
                Latitude = farm.GpsLocation.Latitude,
                Longitude = farm.GpsLocation.Longitude,
                NoOfCages = farm.NoOfCages,
                HasBarge = farm.HasBarge,
                PictureUrl = farm.PictureUrl,
                Workers = new List<WorkerDto>()
            };
        }

        public async Task<FishFarmDto> UpdateFarmAsync(Guid id, UpdateFishFarmDto dto)
        {
            var farm = await _farmRepository.GetByIdAsync(id);
            if (farm == null)
                throw new KeyNotFoundException($"Farm with id {id} not found");

            if (dto.Name != null) farm.Name = dto.Name;
            if (dto.NoOfCages != null) farm.NoOfCages = dto.NoOfCages.Value;
            if (dto.HasBarge != null) farm.HasBarge = dto.HasBarge.Value;
            if (dto.PictureUrl != null) farm.PictureUrl = dto.PictureUrl;
            if (dto.Latitude != null && dto.Longitude != null)
                farm.GpsLocation = new GeoLocation(dto.Latitude.Value, dto.Longitude.Value);

            await _farmRepository.UpdateAsync(farm);
            await _farmRepository.SaveChangesAsync();

            return new FishFarmDto
            {
                Id = id,
                Name = farm.Name,
                Latitude = farm.GpsLocation.Latitude,
                Longitude = farm.GpsLocation.Longitude,
                NoOfCages = farm.NoOfCages,
                HasBarge = farm.HasBarge,
                PictureUrl = farm.PictureUrl,
                Workers = new List<WorkerDto>()
            };

        }

        public async Task DeleteFarmAsync(Guid id)
        {
            var farm = await _farmRepository.GetByIdAsync(id);
            if (farm == null)
                throw new KeyNotFoundException($"Farm with id {id} not found");

            await _farmRepository.DeleteAsync(farm); // make sure this will be soft delete
            await _farmRepository.SaveChangesAsync();
        }

    }
}
