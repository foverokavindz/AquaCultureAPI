using AquaCulture.Application.Dto.Common;
using AquaCulture.Application.DTOs.Common;
using AquaCulture.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaCulture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageUploader _imageUploader;

        public ImageController(IImageUploader imageUploader)
        {
            _imageUploader = imageUploader;
        }

        [HttpGet("sign")]
        public IActionResult GetSignature()
        {
            var signature = _imageUploader.GenerateSignature();
            return Ok(ApiResponseDto<SignatureDto>.SuccessResponse(signature, "Signature is created"));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponseDto<string>.ErrorResponse("No file provided"));

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var result = await _imageUploader.UploadImageAsync(memoryStream, file.FileName);
                return Ok(ApiResponseDto<ImageUploadResponseDto>.SuccessResponse(
                    new ImageUploadResponseDto
                    {
                        Url = result.Url,
                        PublicId = result.PublicId,
                    },
                    "Image uploaded successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{*publicId}")]
        public async Task<IActionResult> Delete(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return BadRequest(ApiResponseDto<bool>.ErrorResponse("No public ID provided"));

            try
            {
                var decodedPublicId = Uri.UnescapeDataString(publicId);

                var result = await _imageUploader.DeleteImageAsync(decodedPublicId);
                if (!result)
                    return NotFound(ApiResponseDto<bool>.ErrorResponse("Image not found"));

                return Ok(ApiResponseDto<bool>.SuccessResponse(true, "Image deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<bool>.ErrorResponse(ex.Message));
            }
        }

    }
}
