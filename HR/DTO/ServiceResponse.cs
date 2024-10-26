namespace HR.DTO
{
    public class ServiceResponse<T>
    {

        public ServiceResponse(T data)
        {
            Data = data;
        }

        public ServiceResponse(T data, bool success, string message, int errorCode)
        {
            Data = data;
            Success = success;
            Message = message;
            ErrorCode = errorCode;
        }

        public ServiceResponse(T data, bool success, string message, int errorCode, string jwt, string refreshToken)
        {
            Data = data;
            Success = success;
            Message = message;
            ErrorCode = errorCode;
            Jwt = jwt;
            RefreshToken = refreshToken;
        }

        public T? Data { get; set; }
        public bool Success { get; set; } = false;
        public bool IsPending { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int ErrorCode { get; set; } = 000000;
        public string? Jwt { get; set; }
        public string? RefreshToken { get; set; }

    }
}
