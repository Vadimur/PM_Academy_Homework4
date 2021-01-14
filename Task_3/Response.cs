using System;

namespace Task_3
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }

        public Response(bool success, T result, string message = null)
        {
            Success = success;
            Result = result;
            Message = message;
        }

        public static Response<T> Error(string message) => new Response<T>(false, default, message);

    }
}