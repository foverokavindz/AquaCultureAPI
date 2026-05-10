using AquaCulture.Application.DTOs.Common;

namespace AquaCulture.Application.Interfaces
{
    public interface IImageUploader
    {
        Task<ImageUploadResponseDto> UploadImageAsync(Stream imageStream, string fileName);
        Task<bool> DeleteImageAsync(string publicId);
        SignatureDto GenerateSignature();
    }
}
