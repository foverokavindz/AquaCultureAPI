using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.Interfaces.Repositories;
using AquaCulture.Application.Interfaces.Services;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Services
{
    public class FishFarmService : IFishFarmService
    {
        private readonly IFishFarmRepository _farmRepository;
        private readonly IWorkerRepository _workerRepository;

        public FishFarmService(IFishFarmRepository farmRepository, IWorkerRepository workerRepository)
        {
            _farmRepository = farmRepository;
            _workerRepository = workerRepository;
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

        public async Task<FishFarmDto> GetFishFarmByIdAsync(Guid id)
        {
            var farm = await _farmRepository.GetFishFarmByIdWithWorkersAsync(id);
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

        public async Task<FishFarmDto> CreateFishFarmAsync(CreateFishFarmDto dto)
        {
            // create the Fish farm
            var fishfarm = new FishFarm
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                GpsLocation = new GeoLocation(dto.Latitude, dto.Longitude),
                NoOfCages = dto.NoOfCages,
                HasBarge = dto.HasBarge,
                PictureUrl = dto.PictureUrl
            };
            await _farmRepository.AddAsync(fishfarm);

            // assign workers if provided
            if (dto.Workers != null && dto.Workers.Any())
            {
                foreach (var w in dto.Workers)
                {
                    var worker = await _workerRepository.GetByIdAsync(w.Id);
                    if (worker == null) 
                        throw new Exception($"Worker with ID {w.Id} not found.");

                    // check the worker is already assigned to another farm
                    if (worker.FishFarmId.HasValue && worker.FishFarmId != fishfarm.Id)
                        throw new InvalidOperationException(
                            $"Worker '{worker.Name}' is already assigned to another fish farm.");

                    worker.FishFarmId = fishfarm.Id;

                    // update role
                    if (w.Position.HasValue)
                        worker.Position = (CrewRole)w.Position.Value;

                    await _workerRepository.UpdateAsync(worker);
                }
               
            }

            await _farmRepository.SaveChangesAsync();
            await _workerRepository.SaveChangesAsync();

            var createdFarm = await _farmRepository.GetFishFarmByIdWithWorkersAsync(fishfarm.Id);

            return new FishFarmDto
            {
                Id = createdFarm.Id,
                Name = createdFarm.Name,
                Latitude = createdFarm.GpsLocation.Latitude,
                Longitude = createdFarm.GpsLocation.Longitude,
                NoOfCages = createdFarm.NoOfCages,
                HasBarge = createdFarm.HasBarge,
                PictureUrl = createdFarm.PictureUrl,
                Workers = createdFarm.Workers.Count > 0 ? createdFarm.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    ProfileImageUrl = w.ProfileImageUrl,
                    Age = w.Age,
                    Email = w.Email,
                    CertifiedUntil = w.CertifiedUntil,
                    FishFarmId = w.FishFarmId,
                    Position = w.Position,
                }).ToList() : []
            };
        }

        public async Task<FishFarmDto> UpdateFishFarmAsync(Guid id, UpdateFishFarmDto dto)
        {
            // Check already avilable
            var fishfarm = await _farmRepository.GetByIdAsync(id);
            if (fishfarm == null)
                throw new KeyNotFoundException($"Farm with id {id} not found");

            // update farm fields
            if (dto.Name != null) fishfarm.Name = dto.Name;
            if (dto.NoOfCages != null) fishfarm.NoOfCages = dto.NoOfCages.Value;
            if (dto.HasBarge != null) fishfarm.HasBarge = dto.HasBarge.Value;
            if (dto.PictureUrl != null) fishfarm.PictureUrl = dto.PictureUrl;
            if (dto.Latitude != null && dto.Longitude != null)
                fishfarm.GpsLocation = new GeoLocation(dto.Latitude.Value, dto.Longitude.Value);

            await _farmRepository.UpdateAsync(fishfarm);

            // handle worker assignments
            if (dto.Workers != null)
            {
                // get current workers assigned to this farm
                var existingWorkers = await _workerRepository.GetWorkersByFarmIdAsync(id);

                var existingWorkerIds = existingWorkers.Select(w => w.Id).ToHashSet();
                var newWorkerIds = dto.Workers.Select(w => w.Id).ToHashSet();

                // workers no longer in new worker id list will unassign
                foreach (var existingWorker in existingWorkers)
                {
                    if (!newWorkerIds.Contains(existingWorker.Id))
                    {
                        existingWorker.FishFarmId = null;
                        existingWorker.Position = null;
                        await _workerRepository.UpdateAsync(existingWorker);
                    }
                }

                // New workers to save
                foreach (var w in dto.Workers)
                {
                    var worker = await _workerRepository.GetByIdAsync(w.Id);
                    if (worker == null) throw new Exception($"Worker with worker id : {w.Id} is not found");

                    // check the worker is already assigned to another farm
                    if (worker.FishFarmId.HasValue && worker.FishFarmId != fishfarm.Id)
                        throw new InvalidOperationException(
                            $"Worker '{worker.Name}' is already assigned to another fish farm.");

                    // assigned them if they not assigned to here already
                    if (!existingWorkerIds.Contains(worker.Id))
                    {
                        worker.FishFarmId = fishfarm.Id;
                    }

                    // update role if provided
                    if (w.Position.HasValue)
                        worker.Position = (CrewRole)w.Position.Value;

                    await _workerRepository.UpdateAsync(worker);
                }

            }

            await _farmRepository.SaveChangesAsync();
            await _workerRepository.SaveChangesAsync();

            var updatedFarm = await _farmRepository.GetFishFarmByIdWithWorkersAsync(fishfarm.Id);

            return new FishFarmDto
            {
                Id = updatedFarm.Id,
                Name = updatedFarm.Name,
                Latitude = updatedFarm.GpsLocation.Latitude,
                Longitude = updatedFarm.GpsLocation.Longitude,
                NoOfCages = updatedFarm.NoOfCages,
                HasBarge = updatedFarm.HasBarge,
                PictureUrl = updatedFarm.PictureUrl,
                Workers = updatedFarm.Workers.Count > 0 ? updatedFarm.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    ProfileImageUrl = w.ProfileImageUrl,
                    Age = w.Age,
                    Email = w.Email,
                    CertifiedUntil = w.CertifiedUntil,
                    FishFarmId = w.FishFarmId,
                    Position = w.Position,
                }).ToList() : []
            };
        }

        public async Task<bool> DeleteFishFarmAsync(Guid id)
        {
            var farm = await _farmRepository.GetByIdAsync(id);
            if (farm == null)
                throw new KeyNotFoundException($"Farm with id {id} not found");

            //unassign all workers from this farm
            var workers = await _workerRepository.GetWorkersByFarmIdAsync(id);

            foreach (var worker in workers)
            {
                worker.FishFarmId = null;
                worker.Position = CrewRole.NotAssigned;
                await _workerRepository.UpdateAsync(worker);
            }

            await _farmRepository.DeleteAsync(farm);
            await _workerRepository.SaveChangesAsync();
            await _farmRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<FishFarmDto>> SearchFishFarmsAsync(SearchFishFarmDto dto)
        {
            var fishfarm = await _farmRepository.SearchFarmsAsync(dto);

            return fishfarm.Select(f => new FishFarmDto
            {
                Id = f.Id,
                Name = f.Name,
                Latitude = f.GpsLocation.Latitude,
                Longitude = f.GpsLocation.Longitude,
                NoOfCages = f.NoOfCages,
                HasBarge = f.HasBarge,
                PictureUrl = f.PictureUrl,
                Workers = f.Workers.Count > 0 ? f.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    ProfileImageUrl = w.ProfileImageUrl,
                    Age = w.Age,
                    Email = w.Email,
                    CertifiedUntil = w.CertifiedUntil,
                    FishFarmId = w.FishFarmId,
                    Position = w.Position,
                }).ToList() : []
            });
        }

    }
}
