namespace MiniSIGVAL.API.Helpers
{
    public class ApiResponse<T>
    {
        public bool Ok { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T? data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Ok = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                Ok = false,
                Message = message,
                Data = default
            };
        }
    }
}