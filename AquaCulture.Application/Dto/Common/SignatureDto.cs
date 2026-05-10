namespace AquaCulture.Application.DTOs.Common
{
    public class SignatureDto
    {
        public string Signature { get; set; } = string.Empty;
        public long Timestamp { get; set; }
        public string CloudName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string Folder { get; set; } = string.Empty;
        public string UploadUrl { get; set; } = string.Empty;
    }
}