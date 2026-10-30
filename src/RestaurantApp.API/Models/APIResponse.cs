using Microsoft.AspNetCore.Http;

namespace RestaurantApp.API.Models
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public static APIResponse<T> Success(T data, int statusCode = StatusCodes.Status200OK)
        {
            return new APIResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static APIResponse<object> Failure(int statusCode, string message, params string[] errors)
        {
            return new APIResponse<object>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                ErrorMessages = errors.ToList()
            };
        }
    }
}
