namespace Mokiniu_registro_api.Services.Responses
{
    public class LoginResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = null;

        public ResponseType? ResponseType { get; set; } = null;
    }

    public enum ResponseType
    {
        Success,
        NotFound,
        BadRequest,
        Forbidden
    }
}
