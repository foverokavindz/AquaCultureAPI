using AquaCulture.Application.Dto.FishFarm;
using AquaCulture.Application.Dto.Common;
using AquaCulture.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaCulture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishFarmController : ControllerBase
    {
        private readonly IFishFarmService _farmService;

        public FishFarmController(IFishFarmService farmService)
        {
            _farmService = farmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFishFarms()
        {
            var farms = await _farmService.GetAllFarmsAsync();

            if (!farms.Any())
                return NotFound(ApiResponseDto<IEnumerable<FishFarmDto>>.ErrorResponse("No fish farms found."));

            return Ok(ApiResponseDto<IEnumerable<FishFarmDto>>.SuccessResponse(farms, $"Retrieved {farms.Count()} fish farm(s) successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishFarmById(Guid id)
        {
            try
            {
                var farm = await _farmService.GetFarmByIdAsync(id);
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
                var farm = await _farmService.CreateFarmAsync(dto);
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
                var farm = await _farmService.UpdateFarmAsync(id, dto);
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
                await _farmService.DeleteFarmAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDto<FishFarmDto>.ErrorResponse(ex.Message));
            }
        }
    }
}