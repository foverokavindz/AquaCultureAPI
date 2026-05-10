using AquaCulture.Application.DTOs.Common;
using AquaCulture.Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace AquaCulture.Infrastructure.Services
{
    public class CloudinaryImageUploader : IImageUploader
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _cloudName;   
        private readonly string _apiKey;     
        private readonly string _folder;
        private readonly string _uploadUrl;

        public CloudinaryImageUploader(IConfiguration configuration)
        {
            _cloudName = configuration["Cloudinary:CloudName"]!;
            _apiKey = configuration["Cloudinary:ApiKey"]!;
            _folder = configuration["Cloudinary:FolderName"]!;
            var baseUrl = configuration["Cloudinary:BaseUrl"]!;
            _uploadUrl = $"{baseUrl}/{_cloudName}/image/upload";

            var apiSecret = configuration["Cloudinary:ApiSecret"]!;

            var account = new Account(_cloudName, _apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
            _cloudinary.Api.Timeout = 60000;
        }

        public SignatureDto GenerateSignature()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var parameters = new SortedDictionary<string, object>
            {
                { "folder", _folder },
                { "timestamp", timestamp }
            };

            var signature = _cloudinary.Api.SignParameters(parameters);

            return new SignatureDto
            {
                Signature = signature,
                Timestamp = timestamp,
                //CloudName = _cloudName,  
                ApiKey = _apiKey,      
                Folder = _folder,
                UploadUrl = _uploadUrl

            };
        }

        public async Task<ImageUploadResponseDto> UploadImageAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, imageStream),
                Folder = "aquaculture"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception($"Image upload failed: {result.Error.Message}");

            return new ImageUploadResponseDto
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

    }
}
