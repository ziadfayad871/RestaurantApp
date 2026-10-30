namespace RestaurantApp_API.Model
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
