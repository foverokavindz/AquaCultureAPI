using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using AquaCulture.Application.Interfaces.Services;

namespace AquaCulture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishFarmController : ControllerBase
    {
        private readonly IFishFarmService _fishfarmService;

        public FishFarmController(IFishFarmService farmService)
        {
            _fishfarmService = farmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFishFarms()
        {
            var farms = await _fishfarmService.GetAllFarmsAsync();

            if (!farms.Any())
                return NotFound(ApiResponseDto<IEnumerable<FishFarmDto>>.ErrorResponse("No fish farms found."));

            return Ok(ApiResponseDto<IEnumerable<FishFarmDto>>.SuccessResponse(farms, $"Retrieved {farms.Count()} fish farm(s) successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishFarmById(Guid id)
        {
            try
            {
                var farm = await _fishfarmService.GetFishFarmByIdAsync(id);
                return Ok(ApiResponseDto<FishFarmDto>.SuccessResponse(farm, $"Retrieved fish farm with ID {id} successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarm([FromBody] CreateFishFarmDto dto)
        {
            try
            {
                var farm = await _fishfarmService.CreateFishFarmAsync(dto);
                return CreatedAtAction(nameof(GetFishFarmById), new { id = farm.Id },
                    ApiResponseDto<FishFarmDto>.SuccessResponse(farm, "Farm created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarm(Guid id, [FromBody] UpdateFishFarmDto dto)
        {
            try
            {
                var farm = await _fishfarmService.UpdateFishFarmAsync(id, dto);
                return Ok(ApiResponseDto<FishFarmDto>.SuccessResponse(farm, "Farm updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarm(Guid id)
        {
            try
            {
                var result = await _fishfarmService.DeleteFishFarmAsync(id);
                if (!result)
                    return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse($"Fish farm with ID {id} not found."));

                return Ok(ApiResponseDto<bool>.SuccessResponse(result, "Farm deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFarms([FromQuery] SearchFishFarmDto dto)
        {
            var farms = await _fishfarmService.SearchFishFarmsAsync(dto);
            return Ok(ApiResponseDto<IEnumerable<FishFarmDto>>.SuccessResponse(farms, $"Retrieved {farms.Count()} fish farms successfully."));
        }
    }
}