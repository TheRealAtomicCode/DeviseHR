namespace OP.DTO
{
    public class ServiceResponse<T>
    {

        public ServiceResponse(T data)
        {
            Data = data;
        }

        public ServiceResponse(T data, bool success, string message, int errorCode, int totalItemCount, int pageSize, int currentPage)
        {
            Data = data;
            Success = success;
            Message = message;
            ErrorCode = errorCode;
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }

        public T? Data { get; set; }
        public bool Success { get; set; } = false;
        public bool IsPending { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int ErrorCode { get; set; } = 000000;
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

    }
}
