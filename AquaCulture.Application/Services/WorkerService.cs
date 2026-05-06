using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.interfaces;
using AquaCulture.Application.Interfaces;
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

        public async Task<IEnumerable<WorkerDto>> GetWorkersByFarmIdAsync(Guid farmId)
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
                FishFarmId = w.FishFarmId
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
                FishFarmId = worker.FishFarmId
            };
        }

        public async Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto dto)
        {
            var farm = await _farmRepository.GetByIdAsync(dto.FishFarmId);
            if (farm == null)
                throw new KeyNotFoundException($"Fish farm with ID {dto.FishFarmId} not found.");

            var worker = new Worker
            {
                Name = dto.Name,
                ProfileImageUrl = dto.ProfileImageUrl,
                Age = dto.Age,
                Email = dto.Email,
                Position = dto.Position,
                CertifiedUntil = dto.CertifiedUntil,
                FishFarmId = dto.FishFarmId
            };

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
                FishFarmId = worker.FishFarmId
            };
        }

        public async Task DeleteWorkerAsync(Guid id)
        {
            var worker = await _workerRepository.GetByIdAsync(id);
            if (worker == null)
                throw new KeyNotFoundException($"Worker with ID {id} not found.");

            await _workerRepository.DeleteAsync(worker); // should be soft delete
            await _workerRepository.SaveChangesAsync();
        }
    }
}