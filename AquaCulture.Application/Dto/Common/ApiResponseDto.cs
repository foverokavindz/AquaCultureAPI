namespace AquaCulture.Application.Dto.Common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; } = null;
        public string? Error { get; set; } = null;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponseDto<object> SuccessResponse(string message) 
        {
            return new ApiResponseDto<object>
            {
                Success = true,
                Message = message
            };
        }

        public static ApiResponseDto<T> SuccessResponse(T data, string message)
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponseDto<object> ErrorResponse(string message)
        {
            return new ApiResponseDto<object>
            {
                Success = false,
                Error = message
            };
        }

        public static ApiResponseDto<T> ErrorResponse(Exception ex)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
