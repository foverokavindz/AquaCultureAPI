using AquaCulture.Application.Dto.Common;
using AquaCulture.Application.Dto.Worker;
using AquaCulture.Application.Interfaces.services;
using AquaCulture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AquaCulture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet("fishfarm/{farmId}")]
        public async Task<IActionResult> GetWorkersByFarmId(Guid farmId)
        {
            var workers = await _workerService.GetWorkersByFishFarmIdAsync(farmId);

            if (!workers.Any())
                return NotFound(ApiResponseDto<IEnumerable<WorkerDto>>.ErrorResponse($"No workers found for fish farm {farmId}."));

            return Ok(ApiResponseDto<IEnumerable<WorkerDto>>.SuccessResponse(workers, $"Retrieved {workers.Count()} worker(s) successfully."));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(Guid id)
        {
            try
            {
                var worker = await _workerService.GetByIdWithFishFarmAsync(id);
                return Ok(ApiResponseDto<WorkerDto>.SuccessResponse(worker, $"Retrieved worker with ID {id} successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<WorkerDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto dto)
        {
            try
            {
                var worker = await _workerService.CreateWorkerAsync(dto);
                return CreatedAtAction(nameof(GetWorkerById), new { id = worker.Id },
                    ApiResponseDto<WorkerDto>.SuccessResponse(worker, "Worker created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<WorkerDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(Guid id, [FromBody] UpdateWorkerDto dto)
        {
            try
            {
                var worker = await _workerService.UpdateWorkerAsync(id, dto);
                return Ok(ApiResponseDto<WorkerDto>.SuccessResponse(worker, "Worker updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<WorkerDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            try
            {
                var result = await _workerService.DeleteWorkerAsync(id);
                if (!result)
                    return NotFound(ApiResponseDto<WorkerDto>.ErrorResponse($"Worker with ID {id} not found."));

                return Ok(ApiResponseDto<bool>.SuccessResponse(result, "Worker deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<WorkerDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchWorkerAsync([FromQuery] SearchWorkerDto dto)
        {
            var workers = await _workerService.SearchWorkerAsync(dto);
            return Ok(ApiResponseDto<IEnumerable<WorkerDto>>.SuccessResponse(workers, $"Retrieved {workers.Count()} workers successfully."));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkers()
        {
            var workers = await _workerService.GetAllWorkersAsync();
            return Ok(ApiResponseDto<IEnumerable<WorkerDto>>.SuccessResponse(workers, $"Retrieved {workers.Count()} fish farm(s) successfully."));
        }
    }
}