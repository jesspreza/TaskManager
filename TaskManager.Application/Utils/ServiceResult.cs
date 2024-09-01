namespace TaskManager.Application.Utils
{
    public class ServiceResult
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }

        protected ServiceResult(bool success, string? message = null)
        {
            Success = success;
            Message = message;
        }

        public static ServiceResult SuccessResult(string? message = null)
        {
            return new ServiceResult(true, message);
        }

        public static ServiceResult Fail(string message)
        {
            return new ServiceResult(false, message);
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; private set; }

        public ServiceResult(bool success, T data, string? message = null)
            : base(success, message)
        {
            Data = data;
        }

        public static ServiceResult<T> SuccessResult(T data, string? message = null)
        {
            return new ServiceResult<T>(true, data, message);
        }

        public static new ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T>(false, default, message);
        }
    }
}
