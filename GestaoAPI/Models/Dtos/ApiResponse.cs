namespace GestaoAPI.Models.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";

        public T? Data { get; set; }

        // SUCESSO
        public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso.")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        // ERRO
        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}