using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.DTOs.FishFarm;
using AquaCulture.Application.Interfaces.Repositories;
using AquaCulture.Application.Interfaces.services;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Application.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IFishFarmRepository _farmRepository;

        public WorkerService(IWorkerRepository workerRepository, IFishFarmRepository farmRepository)
        {
            _workerRepository = workerRepository;
            _farmRepository = farmRepository;
        }

        public async Task<IEnumerable<WorkerDto>> GetWorkersByFishFarmIdAsync(Guid farmId)
        {
            var workers = await _workerRepository.GetWorkersByFarmIdAsync(farmId);

            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                ProfileImageUrl = w.ProfileImageUrl,
                Age = w.Age,
                Email = w.Email,
                Position = w.Position,
                CertifiedUntil = w.CertifiedUntil,
                FishFarmId = w.FishFarmId,
                FishFarm = w.FishFarm != null ? new FishFarmSummaryDto
                {
                    Id = w.FishFarm.Id,
                    Name = w.FishFarm.Name,
                    PictureUrl = w.FishFarm.PictureUrl
                } : null,
            });
        }

        public async Task<WorkerDto> GetWorkerByIdAsync(Guid id)
        {
            var worker = await _workerRepository.GetByIdAsync(id);
            if (worker == null)
                throw new KeyNotFoundException($"Worker with ID {id} not found.");

            return new WorkerDto
            {
                Id = worker.Id,
                Name = worker.Name,
                ProfileImageUrl = worker.ProfileImageUrl,
                Age = worker.Age,
                Email = worker.Email,
                Position = worker.Position,
                CertifiedUntil = worker.CertifiedUntil,
                FishFarmId = worker.FishFarmId,
                FishFarm = worker.FishFarm != null ? new FishFarmSummaryDto
                {
                    Id = worker.FishFarm.Id,
                    Name = worker.FishFarm.Name,
                    PictureUrl = worker.FishFarm.PictureUrl
                } : null,
            };
        }

        public async Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto dto)
        {
            // Create worker
            var worker = new Worker
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProfileImageUrl = dto.ProfileImageUrl,
                Age = dto.Age,
                Email = dto.Email,
                Position = dto.Position,
                CertifiedUntil = dto.CertifiedUntil,
            };

            // assign Fish farm if provided
            if (dto.FishFarmId.HasValue)
            {
                var fishfarm = await _farmRepository.GetByIdAsync(dto.FishFarmId.Value);
                if (fishfarm == null)
                    throw new KeyNotFoundException($"Fish farm with ID {dto.FishFarmId} not found.");

                worker.FishFarmId = fishfarm.Id;
            }

            await _workerRepository.AddAsync(worker);
            await _workerRepository.SaveChangesAsync();

            return new WorkerDto
            {
                Id = worker.Id,
                Name = worker.Name,
                ProfileImageUrl = worker.ProfileImageUrl,
                Age = worker.Age,
                Email = worker.Email,
                Position = worker.Position,
                CertifiedUntil = worker.CertifiedUntil,
                FishFarmId = worker.FishFarmId
            };
        }

        public async Task<WorkerDto> UpdateWorkerAsync(Guid id, UpdateWorkerDto dto)
        {
            var worker = await _workerRepository.GetByIdAsync(id);
            if (worker == null)
                throw new KeyNotFoundException($"Worker with ID {id} not found.");

            if (dto.Name != null) worker.Name = dto.Name;
            if (dto.ProfileImageUrl != null) worker.ProfileImageUrl = dto.ProfileImageUrl;
            if (dto.Age != null) worker.Age = dto.Age.Value;
            if (dto.Email != null) worker.Email = dto.Email;
            if (dto.Position != null) worker.Position = dto.Position.Value;
            if (dto.CertifiedUntil != null) worker.CertifiedUntil = dto.CertifiedUntil.Value;

            await _workerRepository.UpdateAsync(worker);
            await _workerRepository.SaveChangesAsync();

            return new WorkerDto
            {
                Id = worker.Id,
                Name = worker.Name,
                ProfileImageUrl = worker.ProfileImageUrl,
                Age = worker.Age,
                Email = worker.Email,
                Position = worker.Position,
                CertifiedUntil = worker.CertifiedUntil,
                FishFarmId = worker.FishFarmId,
                FishFarm = worker.FishFarm != null ? new FishFarmSummaryDto
                {
                    Id = worker.FishFarm.Id,
                    Name = worker.FishFarm.Name,
                    PictureUrl = worker.FishFarm.PictureUrl
                } : null,
            };
        }

        public async Task<bool> DeleteWorkerAsync(Guid id)
        {
            var worker = await _workerRepository.GetByIdAsync(id);
            if (worker == null)
                throw new KeyNotFoundException($"Worker with ID {id} not found.");

            await _workerRepository.DeleteAsync(worker);
            await _workerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<WorkerDto>> SearchWorkerAsync(SearchWorkerDto dto)
        {
            var workers = await _workerRepository.SearchWorkerAsync(dto);

            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                Age = w.Age,
                Email = w.Email,
                Position = w.Position,
                CertifiedUntil = w.CertifiedUntil,
                ProfileImageUrl = w.ProfileImageUrl,
                FishFarmId= w.FishFarmId,
                FishFarm = w.FishFarm != null ? new FishFarmSummaryDto
                {
                     Id = w.FishFarm.Id,
                     Name = w.FishFarm.Name,
                     PictureUrl = w.FishFarm.PictureUrl
                } : null,
            });
        }

        public async Task<IEnumerable<WorkerDto>> GetAllWorkersAsync()
        {
            var workers = await _workerRepository.GetAllWithFarmAsync();

            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                ProfileImageUrl = w.ProfileImageUrl,
                Age = w.Age,
                Email = w.Email,
                Position = w.Position,
                CertifiedUntil = w.CertifiedUntil,
                FishFarmId = w.FishFarmId,
                FishFarm = w.FishFarm != null ? new FishFarmSummaryDto  
                {
                    Id = w.FishFarm.Id,
                    Name = w.FishFarm.Name,
                    PictureUrl = w.FishFarm.PictureUrl
                } : null,
            });
        }

        public async Task<WorkerDto> GetByIdWithFishFarmAsync(Guid id)
        {
            var worker = await _workerRepository.GetByIdWithFishFarmAsync(id);
            if (worker == null) 
                throw new KeyNotFoundException($"Worker with ID {id} not found.");

            return new WorkerDto
            {
                Id = worker.Id,
                Name = worker.Name,
                ProfileImageUrl = worker.ProfileImageUrl,
                Age = worker.Age,
                Email = worker.Email,
                Position = worker.Position,
                CertifiedUntil = worker.CertifiedUntil,
                FishFarmId = worker.FishFarmId,
                FishFarm = worker.FishFarm != null ? new FishFarmSummaryDto
                {
                    Id = worker.FishFarm.Id,
                    Name = worker.FishFarm.Name,
                    PictureUrl = worker.FishFarm.PictureUrl
                } : null,
            };
        }
    }
}