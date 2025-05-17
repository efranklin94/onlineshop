namespace onlineshop.Features
{
    public class BaseResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public Dictionary<string, string[]> ValidationErrors = [];

        public static BaseResult Fail(string message, Dictionary<string, string[]> validationErrors)
        {
            var result = new BaseResult();
            result.Error(message, validationErrors);

            return result;
        }

        public static BaseResult Success()
        {
            var result = new BaseResult();
            result.Ok("Done");

            return result;
        }

        public static BaseResult<T> Success<T>(T value)
        {
            var result = new BaseResult<T>();

            result.SetValue(value);
            result.Ok("Done");

            return result;
        }

        private void Error(string message, Dictionary<string, string[]> validationErrors)
        {
            IsSuccess = false;
            Message = message;
            ValidationErrors = validationErrors;
        }

        private void Ok(string message)
        {
            IsSuccess = true;
            Message = message;
        }
    }

    public class BaseResult<T> : BaseResult
    {
        public T? Value { get; private set; }

        public void SetValue(T value)
        {
            Value = value;
        }
    }
}
