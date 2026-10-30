namespace RestaurantApp.Application.Common
{
    public enum ServiceResultStatus
    {
        Success = 200,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409
    }

    public class ServiceResult<T>
    {
        public ServiceResultStatus Status { get; init; }
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

        public bool IsSuccess => Status == ServiceResultStatus.Success;

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultStatus.Success,
                Data = data
            };
        }

        public static ServiceResult<T> BadRequest(string message, params string[] errors)
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultStatus.BadRequest,
                Message = message,
                Errors = errors
            };
        }

        public static ServiceResult<T> NotFound(string message)
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultStatus.NotFound,
                Message = message
            };
        }

        public static ServiceResult<T> Conflict(string message)
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultStatus.Conflict,
                Message = message
            };
        }
    }
}
